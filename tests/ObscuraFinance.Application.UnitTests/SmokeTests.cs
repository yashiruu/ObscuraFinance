using FluentAssertions;

namespace ObscuraFinance.Application.UnitTests
{

    public class SmokeTests
    {
        [Fact]
        public void True_Should_BeTrue()
        {
            true.Should().BeTrue();
        }
    }
}
