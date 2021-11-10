using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretAgency.Models
{
    public class Mission
    {
        public Guid Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public IEnumerable<string> Steps { get; init; }
        public IEnumerable<Point> Reward { get; init; }
        public DateTime ValidFromUTC { get; init; }
        public DateTime ValidToUTC { get; init; }
        public bool HasTimeLimit => ValidFromUTC == DateTime.MinValue && ValidToUTC == DateTime.MaxValue;

        public Mission()
        {
            ValidFromUTC = DateTime.MinValue;
            ValidToUTC = DateTime.MaxValue;
        }
    }
}
