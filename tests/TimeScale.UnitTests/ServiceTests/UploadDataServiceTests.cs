using FluentValidation;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Text;
using TimeScale.BLL.Models.Value;
using TimeScale.BLL.Services;
using TimeScale.DAL.Interfaces;
using TimeScale.UnitTests.Common;
using TimeScale.DAL.Entities;
using FluentValidation.Results;
using TimeScale.BLL.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace TimeScale.UnitTests.ServiceTests
{
    public class UploadDataServiceTests : ServiceTestBase<UploadDataService>
    {
        private readonly Mock<IAssessmentRepository> _assessmentRepositoryMock;
        private readonly Mock<IValidator<ValueDto>> _valueValidatorMock; 
        private readonly UploadDataService _uploadDataService;

        public UploadDataServiceTests()
        {
            _assessmentRepositoryMock = new Mock<IAssessmentRepository>();
            _valueValidatorMock = new Mock<IValidator<ValueDto>>();
            _uploadDataService = new UploadDataService(_assessmentRepositoryMock.Object, _valueValidatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task LoadDataFromCSVAsync_ValidCSVFileData_Success()
        {
            //Arrange
            var content = "Date;ExecutionTime;Value\n" +
                      "2025-07-25T22-30-01.0543Z;150;23.4\n" +
                      "2021-09-11T12-23-54.4321Z;200;28.45\n" +
                      "2021-01-11T11-22-51.2321Z;220;21.45";

            var file = CreateCSVData(content);

            _assessmentRepositoryMock
                .Setup(repo => repo.SaveCSVDataAsync(
                    It.IsAny<string>(),
                    It.IsAny<List<ValueEntity>>(),
                    It.IsAny<ResultEntity>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _valueValidatorMock
                .Setup(v => v.ValidateAsync(
                    It.IsAny<ValueDto>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult()); //Valid true by default

            //Act
            await _uploadDataService.LoadDataFromCSVAsync(file);

            //Assert
            _assessmentRepositoryMock.Verify(r => r.SaveCSVDataAsync(
                It.Is<string>(n => n == "test.csv"),
                It.Is<List<ValueEntity>>(list => list.Count == 3 && list[0].Value == 23.4),
                It.Is<ResultEntity>(res =>
                    res.FileName == "test.csv" &&
                    res.MaxValue == 28.45 &&
                    res.AvgValue > 24),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }

        [Fact]
        public async Task LoadDataFromCSVAsync_InvalidCSVFileData_ShouldThrowServiceAppException()
        {
            //Arrange
            var content = "Date;ExecutionTime;Value\n" +
                      "2025-07-25T22-30-01.0543Z;150\n" +
                      "2021-09-11T12-23-54.4321Z;200;28.45\n" +
                      "2021-01-11T11-22-51.2321Z;220;21.45";

            var file = CreateCSVData(content);

            // Act & Assert
            await Assert.ThrowsAsync<ServiceAppException>(() =>
                _uploadDataService.LoadDataFromCSVAsync(file));
        }

        [Fact]
        public async Task LoadDataFromCSVAsync_EmptyCSVFileData_ShouldThrowValidationAppException()
        {
            //Arrange
            var content = "Date;ExecutionTime;Value";

            var file = CreateCSVData(content);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationAppException>(() =>
                _uploadDataService.LoadDataFromCSVAsync(file));
        }


        [Fact]
        public async Task LoadDataFromCSVAsync_ValidatorThrowsException_ShouldThrowValidationAppException()
        {
            //Arrange
            var content = "Date;ExecutionTime;Value\n" +
                      "2021-09-11T12-23-54.4321Z;200;28.45\n" +
                      "2021-01-11T11-22-51.2321Z;220;21.45\n" +
                      "1999-07-25T22-30-01.0543Z;150;23.4";

            var file = CreateCSVData(content);

            var minDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            _valueValidatorMock
                .Setup(v => v.ValidateAsync(
                    It.Is<ValueDto>(v => v.Date < minDate),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("Date", "Error message") }));

            _valueValidatorMock
                .Setup(v => v.ValidateAsync(
                    It.Is<ValueDto>(v => v.Date >= minDate),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            // Act & Assert
            await Assert.ThrowsAsync<ValidationAppException>(() =>
                _uploadDataService.LoadDataFromCSVAsync(file));

            _valueValidatorMock.Verify(v => v.ValidateAsync(
                It.IsAny<ValueDto>(),
                It.IsAny<CancellationToken>()
            ), Times.Exactly(3));

            _valueValidatorMock.Verify(v => v.ValidateAsync(
                It.Is<ValueDto>(v => v.Date < minDate),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }

        [Fact]
        public async Task LoadDataFromCSVAsync_RepositoryThrowsException_ShouldThrowDatabaseOperationException()
        {
            //Arrange
            var content = "Date;ExecutionTime;Value\n" +
                      "2025-07-25T22-30-01.0543Z;150;23.4\n" +
                      "2021-09-11T12-23-54.4321Z;200;28.45\n" +
                      "2021-01-11T11-22-51.2321Z;220;21.45";

            var file = CreateCSVData(content);

            _assessmentRepositoryMock
                .Setup(repo => repo.SaveCSVDataAsync(
                    It.IsAny<string>(),
                    It.IsAny<List<ValueEntity>>(),
                    It.IsAny<ResultEntity>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DbUpdateException());

            _valueValidatorMock
                .Setup(v => v.ValidateAsync(
                    It.IsAny<ValueDto>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            // Act & Assert
            await Assert.ThrowsAsync<DatabaseOperationException>(() =>
                _uploadDataService.LoadDataFromCSVAsync(file));

            _assessmentRepositoryMock.Verify(r => r.SaveCSVDataAsync(
                It.IsAny<string>(),
                It.IsAny<List<ValueEntity>>(),
                It.IsAny<ResultEntity>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }

        private IFormFile CreateCSVData(string content)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            IFormFile file = new FormFile(stream, 0, stream.Length, "test", "test.csv");
            return file;
        }
    }
}
