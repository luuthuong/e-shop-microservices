
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ProductSyncService.Infrastructure.Configs;

public class AppSettingSetup: IConfigureOptions<AppSettings>
{
    private readonly IConfiguration _configuration;

    public AppSettingSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(AppSettings options)
    {
        _configuration.Bind(options);
    }
}