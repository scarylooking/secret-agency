using Microsoft.Extensions.Configuration;
using Moq;
using SecretAgency.Repositories.Interfaces;
using SecretAgency.Services;
using SecretAgency.Services.Interfaces;

namespace SecretAgency.UnitTests.Services
{
    public class MissionServiceTests
    {
        private readonly Mock<IConfiguration> _configuration;
        private readonly Mock<IMissionRepository> _missionRepository;

        private readonly IMissionService _missionService;

        public MissionServiceTests()
        {
            _configuration = new Mock<IConfiguration>();
            _missionRepository = new Mock<IMissionRepository>();

            _missionService = new MissionService(_configuration.Object, _missionRepository.Object);
        }
    }
}