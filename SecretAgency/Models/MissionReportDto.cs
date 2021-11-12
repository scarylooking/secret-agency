using System;

namespace SecretAgency.Models
{
    public record MissionReportDto
    {
        public Guid MissionId { get; init; }
        public string TwitterHandle { get; init; }
        public string FieldNotes { get; init; }
        public string Password { get; init; }
    }
}