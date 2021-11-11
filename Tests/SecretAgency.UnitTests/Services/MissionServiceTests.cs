using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using SecretAgency.Controllers;
using SecretAgency.Repositories;
using SecretAgency.Repositories.Interfaces;
using SecretAgency.Services;
using SecretAgency.Services.Interfaces;
using Xunit;

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

        [Fact]
        public async Task DeleteMission_ReturnsTrue_WhenMissionExists()
        {
            var missionId = Guid.NewGuid();

            _missionRepository.Setup(m => m.Delete(missionId)).ReturnsAsync(true);

            var result = await _missionService.DeleteMission(missionId);

            result.Should().Be(true);
        }
    }
}