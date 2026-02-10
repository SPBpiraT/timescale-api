using Microsoft.Extensions.Logging;
using Moq;
using TimeScale.BLL.Interfaces;

namespace TimeScale.UnitTests.Common
{
    public abstract class ServiceTestBase<T>
        where T : IService
    {
        protected readonly Mock<ILogger<T>> _loggerMock;

        public ServiceTestBase()
        {
            _loggerMock = new Mock<ILogger<T>>();
        }
    }
}
