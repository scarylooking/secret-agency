using Microsoft.Extensions.Configuration;
using Moq;
using SecretAgency.Repositories.Interfaces;
using SecretAgency.Services;
using SecretAgency.Services.Interfaces;

namespace SecretAgency.UnitTests.Services
{
    public class MissionReportServiceTests
    {
        private readonly Mock<IConfiguration> _configuration;
        private readonly Mock<IMissionReportRepository> _missionReportRepository;
        private readonly Mock<IMissionService> _missionService;

        private readonly IMissionReportService _missionReportService;

        public MissionReportServiceTests()
        {
            _configuration = new Mock<IConfiguration>();
            _missionReportRepository = new Mock<IMissionReportRepository>();
            _missionService = new Mock<IMissionService>();

            _missionReportService = new MissionReportService(_configuration.Object, _missionReportRepository.Object, _missionService.Object);
        }
    }
}