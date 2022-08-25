using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(BloodBankApp.Areas.Identity.IdentityHostingStartup))]
namespace BloodBankApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}