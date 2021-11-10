using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace SecretAgency.Models
{
    public class Mission
    {
        [BsonId]
        public Guid Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public IEnumerable<string> Steps { get; init; }
        public IEnumerable<Point> Reward { get; init; }
        public DateTime ValidFromUTC { get; init; }
        public DateTime ValidToUTC { get; init; }

        [BsonIgnore]
        public bool HasTimeLimit => ValidFromUTC != DateTime.MinValue || ValidToUTC != DateTime.MaxValue;

        public Mission()
        {
            ValidFromUTC = DateTime.MinValue;
            ValidToUTC = DateTime.MaxValue;
        }
    }
}
