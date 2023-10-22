using GraphQL;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductSyncService.Domain.Entities;
using ProductSyncService.Infrastructure.Database.Interfaces;

namespace ProductSyncService.Application
{
    public record Employee(int Id, string Name, int Age, int DeptId );

    public record Department(int Id, string Name);

    public class EmployeeDetails
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string DeptName { get; set; } = string.Empty;
    }

    public sealed class EmployeeDetailsType : ObjectGraphType<ProductType>
    {
        public EmployeeDetailsType()
        {                                                                                                                                                     
            Field(x => x.Name);
            Field(x => x.Description);
            Field(x => x.CreatedDate);
            Field(x => x.UpdatedDate);
            Field(x => x.ParentProductTypeId);
        }
    }

    public sealed class EmployeeQuery : ObjectGraphType
    {
        public EmployeeQuery(IAppDbContext dbContext) {
            
            Field<ListGraphType<EmployeeDetailsType>>("Employees")
                .ResolveAsync( async x => await dbContext.ProductType.ToListAsync());
            
            Field<EmployeeDetailsType>("Employee")
                .Argument<Guid>("id")
                .Resolve(
                    x =>
                    {
                        var id = x.GetArgument<Guid>("id");
                        return dbContext.ProductType.FirstOrDefaultAsync(
                            p => p.Id == id);
                    });
        }
    }

    public class EmployeeDetailsSchema : Schema
    {
        public EmployeeDetailsSchema(IServiceProvider serviceProvider) : base(serviceProvider) {
            Query = serviceProvider.GetRequiredService<EmployeeQuery>();
        }
    }
}
