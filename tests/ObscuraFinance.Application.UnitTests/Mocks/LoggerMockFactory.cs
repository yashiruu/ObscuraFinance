using Microsoft.Extensions.Logging;
using Moq;

namespace ObscuraFinance.Application.UnitTests.Mocks
{
    public static class LoggerMockFactory
    {
        public static Mock<ILogger<T>> Create<T>()
        {
            return new Mock<ILogger<T>>();
        }
    }
}
