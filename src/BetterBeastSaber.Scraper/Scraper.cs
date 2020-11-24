
namespace BetterBeastSaber.Scraper
{
    using Domain;

    public class Scraper
    {
        private readonly IBrowsingContext _browsingContext;

        public Scraper(IBrowsingContext browsingContext)
        {
            _browsingContext = browsingContext;
        }

        public async Task ExecuteAsync()
        {
            var baseUrl = "https://bsaber.com/songs/";
            var page = 1;
            bool parsedSongs;

            do
            {
                var navigationUrl = baseUrl + $"page/{page}/";
                var document = await _browsingContext.OpenAsync(navigationUrl);
                var songCellSelector = ".boxed .post";
                var songCells = document.QuerySelectorAll(songCellSelector);

                foreach (var songCell in songCells)
                {
                    var song = ProcessElementToSong(songCell);
                    if (song == null) continue;

                    // TODO Add/Update song to database
                }

                page++;
                parsedSongs = songCells.Length > 0; // TODO Properly detect last page
            } while (parsedSongs);
        }

        private Song ProcessElementToSong(IElement element)
        {
            var titleNode = element.QuerySelector(".entry-title a");
            var id = Regex.Match(titleNode.GetAttribute("href"), @"/(\w+)/[^/]*$").Groups[1].Value;

            // TODO Figure out why the first song on the first place has a funky ID?
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            var (thumbsUp, thumbsDown) = ExtractThumbRating(element);
            var averageUserScore = element.QuerySelector(".circle_rating span")?.TextContent;
            var title = titleNode.GetAttribute("title");

            var song = new Song
            {
                Id = id,
                Title = title,
                ThumbsDown = thumbsDown,
                ThumbsUp = thumbsUp,
                Added = DateTime.Parse(element.QuerySelector(".date.published.time").GetAttribute("datetime")),
                Description = element.QuerySelector(".post-content p").TextContent,
                AverageUserScore = averageUserScore != null ? decimal.Parse(averageUserScore) : (decimal?)null,
                Difficulties = ExtractDifficulties(element),
                Categories = ExtractCategories(element)
            };

            return song;
        }

        private (int ThumbsUp, int ThumbsDown) ExtractThumbRating(IElement element)
        {
            var thumbsUpRoot = element.QuerySelector(".fa-thumbs-up")?.Parent.TextContent.Where(c => char.IsDigit(c)).ToArray();
            var thumbsUp = thumbsUpRoot == null ? 0 : int.Parse(thumbsUpRoot);

            var thumbsDownRoot = element.QuerySelector(".fa-thumbs-down")?.Parent.TextContent.Where(c => char.IsDigit(c)).ToArray();
            var thumbsDown = thumbsDownRoot == null ? 0 : int.Parse(thumbsDownRoot);

            return (thumbsUp, thumbsDown);
        }

        private List<Difficulty> ExtractDifficulties(IElement element)
        {
            var difficultyNodes = element.QuerySelectorAll(".post-difficulty").Select(c => c.TextContent);
            return difficultyNodes.Select(ParseDifficultyText).ToList();
        }

        private Difficulty ParseDifficultyText(string difficultyText)
        {
            return difficultyText switch
            {
                "Expert+" => Difficulty.ExpertPlus,
                _ => (Difficulty)Enum.Parse(typeof(Difficulty), difficultyText),
            };
        }

        private List<string> ExtractCategories(IElement element)
        {
            return element.QuerySelectorAll(".bsaber-categories .single_category_title").Select(c => c.TextContent).ToList();
        }

    }
}
