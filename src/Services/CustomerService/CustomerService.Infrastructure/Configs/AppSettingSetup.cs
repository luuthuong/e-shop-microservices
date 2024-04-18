using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CustomerService.Infrastructure.Configs;

public class AppSettingSetup(IConfiguration configuration) : IConfigureOptions<AppSettings>
{
    public void Configure(AppSettings options)
    {
        configuration.Bind(options);
    }
}