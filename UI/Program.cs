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
        public static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var dbContext = serviceProvider.GetRequiredService<ECommerceDBContext>(); // Replace with your real DbContext name

            string[] roles = { "Customer", "Seller", "Admin" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var emails = new[]
            {
                "alqtta04@gmail.com",
                "yasmeensaffan@gmail.com",
                  "ebtihalali736@gmail.com",
            "aliaamohamed3.2003@gmail.com",
            "alyaamamoon999@gmail.com"
            };

            foreach (var email in emails)
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new ApplicationUser
                    {

                        UserName = email.Split('@')[0],
                        Email = email,
                        EmailConfirmed = true,
                        FirstName = email.Split('@')[0],  // 👈 Add these lines
                        LastName = ""
                    };

                    var result = await userManager.CreateAsync(user, "Default@123");

                    if (!result.Succeeded)
                        continue;
                }

                // Assign all roles
                foreach (var role in roles)
                {
                    if (!await userManager.IsInRoleAsync(user, role))
                    {
                        await userManager.AddToRoleAsync(user, role);
                    }
                }

                // Add to Customer table if not exists
                if (!dbContext.Customers.Any(c => c.ApplicationUserId == user.Id))
                {
                    dbContext.Customers.Add(new Infrastructure.Entites.Customer
                    {
                        ApplicationUserId = user.Id
                    });
                }

                // Add to Seller table if not exists
                if (!dbContext.Sellers.Any(s => s.ApplicationUserId == user.Id))
                {
                    dbContext.Sellers.Add(new Seller
                    {
                        ApplicationUserId = user.Id
                    });
                }

                await dbContext.SaveChangesAsync();
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

            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();
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
          

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
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

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
