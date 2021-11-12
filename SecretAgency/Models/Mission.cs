using System;
using MongoDB.Bson.Serialization.Attributes;

namespace SecretAgency.Models
{
    public record Mission : MissionDto
    {
        [BsonId]
        public Guid Id { get; init; }
        
        [BsonIgnore]
        public bool HasTimeLimit => ValidFromUTC != DateTime.MinValue || ValidToUTC != DateTime.MaxValue;

        public Mission()
        {
            Id = Guid.NewGuid();
        }
    }
}
