using WebsiteXemPhim.Models;
using Xunit;

public class PhimBoTests
{
        [Fact]
        public void PhimBo_DefaultLuotXem_ShouldBeZero()
        {
            // Arrange
            var phim = new PhimBo();

            // Act
            var luotXem = phim.LuotXem;

            // Assert
            Assert.Equal(0, luotXem);
        }
}