using System;
using MongoDB.Bson.Serialization.Attributes;
using SecretAgency.Constants;

namespace SecretAgency.Models
{
    public record MissionReport : MissionReportDto
    {
        [BsonId]
        public Guid Id { get; init; }

        public DateTime CreatedAt { get; }
        public MissionReportApprovalState State { get; init; }

        public MissionReport()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            State = MissionReportApprovalState.Pending;
        }
    }
}
