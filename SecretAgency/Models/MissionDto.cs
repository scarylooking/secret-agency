using System;
using System.Collections.Generic;

namespace SecretAgency.Models
{
    public record MissionDto
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public IEnumerable<string> Steps { get; init; }
        public IEnumerable<Point> Reward { get; init; }
        public DateTime ValidFromUTC { get; init; }
        public DateTime ValidToUTC { get; init; }

        public MissionDto()
        {
            ValidFromUTC = DateTime.MinValue;
            ValidToUTC = DateTime.MaxValue;
        }
    }
}