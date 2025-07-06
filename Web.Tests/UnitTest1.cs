using WebsiteXemPhim.Models;
using Xunit;

public class PhimBoTests
{
    [Fact]
    public void TenPhimVaLuotXem_ShouldBeCorrect()
    {
        var phimBo = new PhimBo
        {
            TenPhim = "Breaking Bad",
            NoiDung = "Một giáo viên hóa học chế ma túy",
            LuotXem = 10000,
            Like = 5000
        };

        Assert.Equal("Breaking Bad", phimBo.TenPhim);
        Assert.Equal("Một giáo viên hóa học chế ma túy", phimBo.NoiDung);
        Assert.Equal(10000, phimBo.LuotXem);
        Assert.Equal(5000, phimBo.Like);
    }
}