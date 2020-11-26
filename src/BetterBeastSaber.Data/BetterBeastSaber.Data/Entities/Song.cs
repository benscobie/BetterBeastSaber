﻿using BetterBeastSaber.Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace BetterBeastSaber.Data.Entities
{
    public class Song
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }

        public List<Difficulty> Difficulties { get; set; }

        public string Title { get; set; }

        public int ThumbsUp { get; set; }

        public int ThumbsDown { get; set; }

        public DateTime Added { get; set; }

        public List<string> Categories { get; set; }

        public decimal? AverageUserScore { get; set; }

        public string Description { get; set; }

        public UserRating UserRating { get; set; }
    }
}