using EcommerceIOT.Client.Components;
using EcommerceIOT.Client.Services;
using Microsoft.Extensions.Options;

namespace EcommerceIOT.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7013/") });

            builder.Services.AddScoped<AccountServices>();
            builder.Services.AddScoped<ContactServices>();
            builder.Services.AddScoped<ProfileServices>();
            builder.Services.AddScoped<IntroducesServices>();
            builder.Services.AddScoped<ProductServices>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
