using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using BetterBeastSaber.Domain;

namespace BetterBeastSaber.Scraper
{
    public class Extractor
    {
        private readonly IDocument _document;

        public Extractor(IDocument document)
        {
            _document = document;
        }

        public List<Song> ExtractSongs()
        {
            var songCellSelector = ".boxed .post";
            var songCells = _document.QuerySelectorAll(songCellSelector);

            return songCells
                .Select(ExtractSong)
                .Where(song => song != null)
                .ToList();
        }

        private Song ExtractSong(IElement element)
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
                Description = element.QuerySelector(".post-content p")?.TextContent,
                AverageUserScore = averageUserScore != null ? decimal.Parse(averageUserScore) : (decimal?) null,
                Difficulties = ExtractDifficulties(element),
                Categories = ExtractCategories(element)
            };

            return song;
        }

        private (int ThumbsUp, int ThumbsDown) ExtractThumbRating(IElement element)
        {
            var thumbsUpRoot = element.QuerySelector(".fa-thumbs-up")?.Parent.TextContent.Where(char.IsDigit).ToArray();
            var thumbsUp = thumbsUpRoot == null ? 0 : int.Parse(thumbsUpRoot);

            var thumbsDownRoot = element.QuerySelector(".fa-thumbs-down")?.Parent.TextContent.Where(char.IsDigit).ToArray();
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
                _ => (Difficulty) Enum.Parse(typeof(Difficulty), difficultyText),
            };
        }

        private List<string> ExtractCategories(IElement element)
        {
            return element
                .QuerySelectorAll(".bsaber-categories .single_category_title")
                .Select(c => c.TextContent)
                .ToList();
        }
    }
}