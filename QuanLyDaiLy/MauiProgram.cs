using Microsoft.Extensions.Logging;
using QuanLyDaiLy.Data;
using Microsoft.EntityFrameworkCore;
using QuanLyDaiLy.Views.AgencyList;
using QuanLyDaiLy.Views.AgencyAdd;

namespace QuanLyDaiLy
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("fontello.ttf", "MyIcons");
                });

            // Đăng ký DbContext
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "QuanLyDaiLy.db3");
            builder.Services.AddDbContext<DataContext>(options =>
                options.UseSqlite($"Filename={dbPath}"));

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddTransient<Agency>();
            builder.Services.AddTransient<AgencyAdd>();
            builder.Services.AddTransient<Views.AgencyList.Agency>();
            var app = builder.Build();

            // 🔥 Tạo DB tại đây (sau khi DI đã sẵn sàng)
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DataContext>();
                db.Database.EnsureCreated();
            }

            return app;
        }
    }
}
