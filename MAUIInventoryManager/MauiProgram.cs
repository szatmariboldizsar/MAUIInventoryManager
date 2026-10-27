using DAL;
using DAL.Models;
using DAL.Services;
using DAL.Services.Interfaces;
using MAUIInventoryManager.Pages;
using MAUIInventoryManager.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MAUIInventoryManager
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
                });

#if DEBUG
    		builder.Logging.AddDebug();

#endif
            string appDataPath = FileSystem.Current.AppDataDirectory;

            #if WINDOWS
                appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            #endif

            string folderName = "MAUIInventoryManager";
            string targetFolder = Path.Combine(appDataPath, folderName);

            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            string dataFilePath = Path.Combine(targetFolder, "inventory.db");
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite($"Data Source={dataFilePath}"));

            builder.Services.AddTransient<IProductService, ProductService>();

            builder.Services.AddTransient<InventoryPage>();
            builder.Services.AddTransient<InventoryViewModel>();

            builder.Services.AddTransient<ProductDetailPage>();
            builder.Services.AddTransient<ProductDetailViewModel>();

            MauiApp app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                dbContext.Database.EnsureCreated();
            }

            return app;
        }
    }
}
