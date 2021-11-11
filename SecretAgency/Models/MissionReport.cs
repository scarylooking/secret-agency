using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace SecretAgency.Models
{
    public enum MissionReportApprovalState
    {
        Pending,
        Approved,
        Rejected
    }

    public record MissionReport
    {
        [BsonId]
        public Guid Id { get; init; }
        public Guid MissionId { get; init; }
        public string TwitterHandle { get; init; }
        public string FieldNotes { get; init; }
        public string Password { get; init; }
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
