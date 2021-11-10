using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace SecretAgency.Models
{
    public class Agent
    {
        [BsonId]
        public string Username { get; init; }
        public Dictionary<string, int> Balances { get; init; }
    }
}
