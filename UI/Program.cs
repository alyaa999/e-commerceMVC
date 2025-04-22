using e_commerce.Application.Common.Interfaces;
using e_commerce.Domain.Entites;
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
using e_commerce.Web.Controllers;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace e_commerce
{
    public class Program
    {
        public static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var dbContext = serviceProvider.GetRequiredService<ECommerceDBContext>();

            string[] roles = { "Customer", "Seller", "Admin" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // ✅ إنشاء Admin User للتجربة
            var adminEmail = "admin@evava.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "adminuser",
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FirstName = "Admin",
                    LastName = "User"
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123"); // ✅ باسورد واضح وسهل للتجربة

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            await dbContext.SaveChangesAsync();
        }




        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddAutoMapper(typeof(ProductProfile));

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllersWithViews(
                conf => conf.Filters.Add(new AuthorizeFilter())
                );
            // builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddDbContext<ECommerceDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            #region AddServices
            builder.Services.AddScoped<IHomeRepository, HomeRepository>();
            #endregion
            builder.Services.AddScoped<IWishlistRepo, WishlistRepo>();

            builder.Services.AddScoped<ICustRepo, custRepo>();
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
                    options.ClientId = "1075456295809-7vccccvq60amse5uhidjonvt8jimq0l7.apps.googleusercontent.com";
                    options.ClientSecret = "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";
                });

            builder.Services.AddAuthentication()
                .AddFacebook(options =>
                {
                    options.ClientId = "697162356148355";
                    options.ClientSecret = "719008d493e69543fac4bd48c2065aae";
                 
                });






            builder.Services.AddScoped<IcartRepository, CarRepoService>();
            builder.Services.AddScoped<IAdressRepo, AddressRepo>();
            builder.Services.AddAutoMapper(typeof(AddressProfile));
            builder.Services.AddScoped<IOrderRepository, OrderRepoService>();
            builder.Services.AddScoped<LayoutDataFilterAttribute>();



            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            builder.Services.AddScoped<IEmailSenderService, EmailSender>();



            builder.Services.AddScoped<IReturnRepository, returnRepoService>();
            builder.Services.AddScoped(typeof(IRepository<>),typeof(Repository<>));
            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await SeedRolesAndUsersAsync(services);
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


            app.MapControllerRoute(
                name: "externalLogin",
                pattern: "signin-{provider}",
                defaults: new { controller = "Account", action = "ExternalLoginCallback" });



            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
