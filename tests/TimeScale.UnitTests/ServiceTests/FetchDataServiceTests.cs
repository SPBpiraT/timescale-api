using Microsoft.EntityFrameworkCore;
using Moq;
using TimeScale.BLL.Exceptions;
using TimeScale.BLL.Services;
using TimeScale.DAL.Entities;
using TimeScale.DAL.Interfaces;
using TimeScale.Shared.Models;
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
        public async Task GetFilteredResultsAsync_WhenEntitiesExist_ShouldReturnMappedDtos()
        {
            // Arrange
            var filter = new ResultFilterDto { FileName = "test.csv" };

            var entities = new List<ResultEntity>
            {
                new() 
                {
                    Id = 1,
                    FileName = "test.csv",
                    DeltaTime = 0.5,
                    FirstOperationDate = new DateTime(2024, 5, 20, 10, 0, 0, DateTimeKind.Utc),
                    AvgValue = 95.5
                }
            };

            _assessmentRepositoryMock
                .Setup(repo => repo.GetFilteredResultsAsync(filter, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entities);

            // Act
            var result = await _fetchDataService.GetFilteredResultsAsync(filter);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            var dto = Assert.Single(result);

            Assert.Equal(entities[0].FileName, dto.FileName);
            Assert.Equal(entities[0].FirstOperationDate, dto.FirstOperationDate);
            Assert.Equal(DateTimeKind.Utc, dto.FirstOperationDate.Kind);
            Assert.Equal(entities[0].AvgValue, dto.AvgValue);

            _assessmentRepositoryMock.Verify(r => r.GetFilteredResultsAsync(filter, It.IsAny<CancellationToken>()), Times.Once);
        }


        [Fact]
        public async Task GetFilteredResultsAsync_WhenEntitiesNotExist_ShouldReturnEmptyDtosList()
        {
            // Arrange
            var filter = new ResultFilterDto { FileName = "test.csv" };

            var entities = new List<ResultEntity>();

            _assessmentRepositoryMock
                .Setup(repo => repo.GetFilteredResultsAsync(filter, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entities);

            // Act
            var result = await _fetchDataService.GetFilteredResultsAsync(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _assessmentRepositoryMock.Verify(r => r.GetFilteredResultsAsync(filter, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetFilteredResultsAsync_RepositoryThrowsException_ShouldThrowException()
        {
            // Arrange
            var filter = new ResultFilterDto { FileName = "test.csv" };

            _assessmentRepositoryMock
                .Setup(repo => repo.GetFilteredResultsAsync(filter, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DbUpdateException());

            // Act & Assert
            await Assert.ThrowsAsync<DatabaseOperationException>(() =>
                _fetchDataService.GetFilteredResultsAsync(filter));

            _assessmentRepositoryMock.Verify(r =>
                r.GetFilteredResultsAsync(filter, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetLastValuesAsync_WhenEntitiesExist_ShouldReturnMappedDtos()
        {
            //Arrange
            string fileName = "test.csv";

            var entities = new List<ValueEntity>
            {
                new()
                {
                    Id = 1,
                    FileName = "test.csv",
                    Value = 90.0
                },
                new()
                {
                    Id = 2,
                    FileName = "test.csv",
                    Value = 91.0
                }
            };

            _assessmentRepositoryMock
                .Setup(repo => repo.GetLastValuesAsync(fileName, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entities);

            //Act
            var result = await _fetchDataService.GetLastValuesAsync(fileName);

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.True(result.Count() > 1);

            var dto = result.FirstOrDefault();
            Assert.Equal(entities[0].FileName, dto.FileName);
            Assert.Equal(entities[0].Value, dto.Value);

            _assessmentRepositoryMock.Verify(r => r.GetLastValuesAsync(fileName, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetLastValuesAsync_WhenEntitiesNotExist_ShouldReturnEmptyDtosList()
        {
            //Arrange
            string fileName = "test.csv";

            var entities = new List<ValueEntity>();

            _assessmentRepositoryMock
               .Setup(repo => repo.GetLastValuesAsync(fileName, It.IsAny<CancellationToken>()))
               .ReturnsAsync(entities);

            // Act
            var result = await _fetchDataService.GetLastValuesAsync(fileName);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _assessmentRepositoryMock.Verify(r => r.GetLastValuesAsync(fileName, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetLastValuesAsync_RepositoryThrowsException_ShouldThrowException()
        {
            // Arrange
            string fileName = "test.csv";

            _assessmentRepositoryMock
                .Setup(repo => repo.GetLastValuesAsync(fileName, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DbUpdateException());

            // Act & Assert
            await Assert.ThrowsAsync<DatabaseOperationException>(() =>
                _fetchDataService.GetLastValuesAsync(fileName));

            _assessmentRepositoryMock.Verify(r =>
                r.GetLastValuesAsync(fileName, It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}
