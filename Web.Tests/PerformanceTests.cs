using System.Threading;
using Xunit;

namespace Web.Tests
{
    //Parallel Test Execution
    public class PerformanceTests
    {
        [Fact]
        public void Fake_Performance_Test1()
        {
            Thread.Sleep(1000); // giả lập test tốn thời gian
            Assert.True(true);
        }

        [Fact]
        public void Fake_Performance_Test2()
        {
            Thread.Sleep(1000);
            Assert.True(true);
        }
        [Fact]
        public void Test_Should_Fail()
        {
            Assert.True(false); // Test này sẽ luôn fail
        }
    }
}
