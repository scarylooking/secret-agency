namespace SecretAgency.Constants
{
    public static class Errors
    {
        public const string MissionNotFound = "MISSION_NOT_FOUND";
        public const string MissionAlreadyExists = "MISSION_WITH_SAME_ID_ALREADY_EXISTS";
        public const string MissionNotAcceptingReports = "MISSION_NOT_ACCEPTING_REPORTS";
        public const string UnableToDeleteMission = "UNABLE_TO_DELETE_MISSION";

        public const string MissionReportNotFound = "MISSION_REPORT_NOT_FOUND";
        public const string MissionReportAlreadyExists = "MISSION_REPORT_WITH_SAME_ID_ALREADY_EXISTS";
        public const string UnableToDeleteMissionReport = "UNABLE_TO_DELETE_MISSION_REPORT";

        public const string UnknownError = "UNEXPECTED_UNKNOWN_ERROR";
        public const string DatabaseError = "UNEXPECTED_DATABASE_ERROR";
    }
}