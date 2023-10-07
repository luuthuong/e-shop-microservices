using Core.BaseDbContext;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Interface;

public interface IAppDbContext: IBaseDbContext
{
    DbSet<Category> Category { get; set; }
}