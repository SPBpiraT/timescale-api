using Moq;
using TimeScale.BLL.Services;
using TimeScale.DAL.Interfaces;
using TimeScale.UnitTests.Common;

namespace TimeScale.UnitTests.ServiceTests
{
    public class FetchDataServiceTests : ServiceTestBase<FetchDataService>
    {
        private readonly Mock<IAssessmentRepository> _assessmentRepositoryMock;
        private readonly FetchDataService _fetchDataService;

        public FetchDataServiceTests()
        {
            _assessmentRepositoryMock = new Mock<IAssessmentRepository>();
            _fetchDataService = new FetchDataService(_assessmentRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetFilteredResultsAsync_Success()
        {
            //Arrange
            
            //Act

            //Assert
        }

        [Fact]
        public async Task GetLastValuesAsync_Success()
        {
            //Arrange

            //Act

            //Assert
        }
    }
}
