using Core.Utils;
using EntityGraphQL.AspNet;
using EntityGraphQL.Schema;
using EntityGraphQL.Schema.FieldExtensions;
using ProductSyncService.Application.GraphQL.Mutations;
using ProductSyncService.Infrastructure.Database.Interfaces;

namespace API;

public static class Extension
{
    public static IServiceCollection GraphQLRegister(this IServiceCollection services)
    {
        services.AddGraphQLSchema<IAppDbContext>(options =>
        {
            options.PreBuildSchemaFromContext = schema =>
            {
                schema.AddScalarType<KeyValuePair<string, string>>("StringKeyValuePair", "Represents a pair of strings");
            };
        });
        
        var serviceProvider = services.BuildServiceProvider();
        //schema builder
        var schema = serviceProvider.GetRequiredService<SchemaProvider<IAppDbContext>>();
        schema.AddMutationsFrom<ProductMutations>();
        schema.AddMutationsFrom<ProductTypeMutation>();

        schema.Query().ReplaceField(
            "product", context => context.Product, "return list of products").UseFilter().UseSort();
        
        var executionOptions = new EntityGraphQL.Schema.ExecutionOptions();
        executionOptions.BeforeExecuting += (expression, isFinal) =>
        {
            return ExpressionOptimizer.visit(expression);
        };
        
        return services;
    }

    public static IServiceCollection GrpcRegister(this IServiceCollection services)
    {
        services.AddGrpc();
        services.AddGrpcReflection();
        return services;
    }
}