using FluentValidation;
using Moq;
using TimeScale.BLL.Models.Value;
using TimeScale.BLL.Services;
using TimeScale.DAL.Interfaces;
using TimeScale.UnitTests.Common;

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
        public async Task LoadDataFromCSVAsync_ValidFile_Success()
        {
            //Arrange

            //Act

            //Assert
        }

        [Fact]
        public async Task LoadDataFromCSVAsync_InvalidFile_Fail()
        {
            //Arrange

            //Act

            //Assert
        }
    }
}
