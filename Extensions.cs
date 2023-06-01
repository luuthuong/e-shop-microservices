using System.Text.Json;
using Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Domain;

public static class Extensions
{
    public static string GetDbConnection()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
 
        string strConnection = builder.Build().GetConnectionString("Database");
        return strConnection;
    }

    public static string ToJson(this BaseEntity entity)
    {
        return JsonSerializer.Serialize(Convert.ChangeType(entity, entity.GetType()));
    }
}