using e_commerce.Application.Common.Interfaces;
using e_commerce.Domain.Entites;
using e_commerce.Application.Common.Interfaces;
using e_commerce.Infrastructure.Entites;
using e_commerce.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Authorization;
using e_commerce.Infrastructure.Repository;
using e_commerce.Web.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Stripe;

namespace e_commerce
{
    public class Program
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = { "Customer", "Seller", "Admin" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }


        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddAutoMapper(typeof(ProductProfile));
            builder.Services.AddControllersWithViews(
                conf => conf.Filters.Add(new AuthorizeFilter())
                );
            // builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddDbContext<ECommerceDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            #region AddServices
            builder.Services.AddScoped<IHomeRepository, HomeRepository>();
            #endregion
            builder.Services.AddScoped<IWishlistRepo, WishlistRepo>();
            //builder.Services.AddApplicationServices();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
            })
                .AddEntityFrameworkStores<ECommerceDBContext>()
                .AddDefaultTokenProviders();






            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            });

            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/Login"; // must match your controller/action
            });


            builder.Services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = "301550065314-ec3i6m9102daudbhr0nvi075j5ne9hd2.apps.googleusercontent.com";
                    options.ClientSecret = "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";
                });

            builder.Services.AddAuthentication()
                .AddFacebook(options =>
                {
                    options.ClientId = "612541185137585";
                    options.ClientSecret = "21d28beaac7ae0f5473dc0c923606798";
                });




            builder.Services.AddScoped<IcartRepository, CarRepoService>();
            builder.Services.AddScoped<IAdressRepo, AddressRepo>();
            builder.Services.AddAutoMapper(typeof(AddressProfile));
            builder.Services.AddScoped<IOrderRepository, OrderRepoService>();
          
            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await SeedRolesAsync(services);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseRouting();

            app.UseAuthentication();
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
