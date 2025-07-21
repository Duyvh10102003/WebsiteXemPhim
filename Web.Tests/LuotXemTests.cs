using System.Threading;
using Xunit;

namespace Web.Tests
{
    // Test CodeBuild trigger
    public class LuotXemTests
    {
        [Fact]
        public void Fake_LuotXem_Test1()
        {
            Thread.Sleep(1000); // giả lập test mất 1 giây
            Assert.True(true);
        }

        [Fact]
        public void Fake_LuotXem_Test2()
        {
            Thread.Sleep(1000);
            Assert.True(true);
        }
    }
}