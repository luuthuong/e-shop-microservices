using GraphQL.Types;

namespace Infrastructure.GraphQL;

public class JobSchema: Schema
{
    public JobSchema(IServiceProvider services) : base(services)
    {
        // Query = services.GetService()
    }
}