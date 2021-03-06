using System;
using FindProperty.Areas.Identity.Data;
using FindProperty.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(FindProperty.Areas.Identity.IdentityHostingStartup))]
namespace FindProperty.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<FindPropertyContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("FindPropertyContextConnection")));

                services.AddDefaultIdentity<FindPropertyUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<FindPropertyContext>();
            });
        }
    }
}