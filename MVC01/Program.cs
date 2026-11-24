using MVC01.CustomMiddleware;

namespace MVC01
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // 1. Inline Middleware 
            app.Use(async (context, next) =>
            {
                Console.WriteLine("Hello from Inline Middleware (Before)");
                await next();
                Console.WriteLine("Bye from Inline Middleware (After)");
            });

            // 2.  Custom Middleware
            app.UseMiddleware<RequestTimingMiddleware>();


            app.Map("/Hamda", branch =>
            {
                branch.Run(async context =>
                {
                    await context.Response.WriteAsync("Welcome to Hamda Branch!");
                });
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();

            // Custom Conventional Route
            app.MapControllerRoute(
                name: "shop",
                pattern: "shop/{id}",
                defaults: new { controller = "Product", action = "GetProductById" });


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id:int?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
