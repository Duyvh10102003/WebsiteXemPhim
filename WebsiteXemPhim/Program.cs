using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.Configuration;
using System.Configuration;
using WebsiteXemPhim.DataAccess;
using WebsiteXemPhim.Models;
using WebsiteXemPhim.Repositories;
using WebsiteXemPhim.Repository;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<AppUser, IdentityRole>()
.AddDefaultTokenProviders()
.AddDefaultUI()
.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddAuthentication()
    .AddGoogle(googleOptions =>
    {
        // Đọc thông tin Authentication:Google từ appsettings.json
        IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");

        // Thiết lập ClientID và ClientSecret để truy cập API google
        googleOptions.ClientId = googleAuthNSection["ClientId"];
        googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
        // Cấu hình Url callback lại từ Google (không thiết lập thì mặc định là /signin-google)
        googleOptions.CallbackPath = "/signin-google";

    });
   /* .AddFacebook(facebookOptions => {
        // Đọc cấu hình
        IConfigurationSection facebookAuthNSection = builder.Configuration.GetSection("Authentication:Facebook");
        facebookOptions.AppId = facebookAuthNSection["AppId"];
        facebookOptions.AppSecret = facebookAuthNSection["AppSecret"];
        // Thiết lập đường dẫn Facebook chuyển hướng đến
        //facebookOptions.CallbackPath = "/signin-facebook";
    }); */
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IPhimLeRepository, EFPhimLeRepository>();
builder.Services.AddScoped<IPhimBoRepository, EFPhimBoRepository>();
builder.Services.AddScoped<INamRepository, EFNamRepository>();
builder.Services.AddScoped<IQuocGiaRepository, EFQuocGiaRepository>();
builder.Services.AddScoped<ITheLoaiRepository, EFTheLoaiRepository>();
builder.Services.AddScoped<ITapPhimRepository, EFTapPhimRepository>();
builder.Services.AddScoped<ILichSuXemRepository, EFLichSuXemRepository>();
builder.Services.AddScoped<IHopPhimRepository, EFHopPhimRepository>();
builder.Services.AddScoped<ITrangThaiRepository, EFTrangThaiRepository>();

builder.Services.AddOptions();                                        // Kích hoạt Options
var mailSettingsSection = builder.Configuration.GetSection("MailSettings");  // đọc config
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));              // đăng ký để Inject

builder.Services.AddTransient<IEmailSender, SendMailService>();        // Đăng ký dịch vụ Mail

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=PhimLes}/{action=Index}/{id?}"
    );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
