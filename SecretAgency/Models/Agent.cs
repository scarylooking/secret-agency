using System;
using System.Collections.Generic;

namespace SecretAgency.Models
{
    public class Agent
    {
        public string Username { get; init; }
        public Dictionary<string, int> Balances { get; init; }
    }
}
