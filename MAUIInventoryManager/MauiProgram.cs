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

            builder.Services.AddScoped<IProductService, ProductService>();

            builder.Services.AddTransient<InventoryPage>();
            builder.Services.AddTransient<InventoryViewModel>();

            builder.Services.AddTransient<ProductDetailPage>();
            builder.Services.AddTransient<ProductDetailViewModel>();

            MauiApp app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                dbContext.Database.EnsureCreated();

                if (!dbContext.Products.Any())
                {
                    dbContext.Products.AddRange(
                        new Product { Name = "Tej 2.8% 1l", Barcode = "5991234567890", Price = 450, Quantity = 50 },
                        new Product { Name = "Kenyér (Félbarna)", Barcode = "5991234567891", Price = 680, Quantity = 25 },
                        new Product { Name = "Sajt (Trappista)", Barcode = "5991234567892", Price = 2490, Quantity = 15 },
                        new Product { Name = "Tojás (M-es, 10db)", Barcode = "5991234567893", Price = 890, Quantity = 0 },
                        new Product { Name = "Csirkemell filé (kg)", Barcode = "5991234567894", Price = 2190, Quantity = 30 },
                        new Product { Name = "Kávé (Omnia 250g)", Barcode = "5991234567895", Price = 1750, Quantity = 20 },
                        new Product { Name = "Ásványvíz (Szentkirályi 1.5l)", Barcode = "5991234567896", Price = 220, Quantity = 100 },
                        new Product { Name = "Alma (Idared kg)", Barcode = "5991234567897", Price = 590, Quantity = 45 },
                        new Product { Name = "Tésztaleves (Maggi)", Barcode = "5991234567898", Price = 350, Quantity = 60 },
                        new Product { Name = "Csokoládé (Milka tejcsoki)", Barcode = "5991234567899", Price = 550, Quantity = 35 }
                    );

                    dbContext.SaveChanges();
                }
            }

            return app;
        }
    }
}
