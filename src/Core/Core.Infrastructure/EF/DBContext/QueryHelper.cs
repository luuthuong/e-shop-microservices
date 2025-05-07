using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.EF.DBContext;

public static class QueryHelper
{
    private static readonly MethodInfo OrderByMethod =
        typeof(Queryable).GetMethods().Single(method =>
            method.Name == "OrderBy" && method.GetParameters().Length == 2);

    private static readonly MethodInfo OrderByDescendingMethod =
        typeof(Queryable).GetMethods().Single(method =>
            method.Name == "OrderByDescending" && method.GetParameters().Length == 2);

    private static bool PropertyExists<T>(this IQueryable<T> source, string propertyName)
    {
        return typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase |
                                                   BindingFlags.Public | BindingFlags.Instance) != null;
    }

    private static IQueryable<T>? OrderByProperty<T>(
        this IQueryable<T> source, string propertyName)
    {
        if (typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase |
                                                BindingFlags.Public | BindingFlags.Instance) == null)
        {
            return null;
        }

        ParameterExpression parameterExpression = Expression.Parameter(typeof(T));
        Expression orderByProperty = Expression.Property(parameterExpression, propertyName);
        LambdaExpression lambda = Expression.Lambda(orderByProperty, parameterExpression);
        MethodInfo genericMethod =
            OrderByMethod.MakeGenericMethod(typeof(T), orderByProperty.Type);
        object? ret = genericMethod.Invoke(null, new object[] { source, lambda });
        return (IQueryable<T>)ret!;
    }

    private static IQueryable<T>? OrderByPropertyDescending<T>(
        this IQueryable<T> source, string propertyName)
    {
        if (typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase |
                                                BindingFlags.Public | BindingFlags.Instance) == null)
        {
            return null;
        }

        ParameterExpression parameterExpression = Expression.Parameter(typeof(T));
        Expression orderByProperty = Expression.Property(parameterExpression, propertyName);
        LambdaExpression lambda = Expression.Lambda(orderByProperty, parameterExpression);
        MethodInfo genericMethod =
            OrderByDescendingMethod.MakeGenericMethod(typeof(T), orderByProperty.Type);
        object? ret = genericMethod.Invoke(null, [source, lambda]);
        return (IQueryable<T>)ret!;
    }

    public static IQueryable<T>? OrderBySortExpression<T>(this IQueryable<T>? source, string sortTerm)
    {
        string[] terms = sortTerm.Split('_');
        if (terms.Length != 2) return source;
        string fieldName = terms[0];
        string direction = terms[1].ToLower();

        if (source is null)
            return null;

        if (!source.PropertyExists(fieldName))
            return source;

        if (direction == "asc")
        {
            return source.OrderByProperty(fieldName);
        }

        return source.OrderByPropertyDescending(fieldName);
    }

    public static IQueryable<TEntity> WhereIf<TEntity, TKey>(this IQueryable<TEntity> queryable, bool criteria,
        Expression<Func<TEntity, bool>> predicate)
        where TKey : StronglyTypeId<Guid>
        where TEntity : BaseEntity<TKey> =>
        !criteria ? queryable.AsSplitQuery() : queryable.Where(predicate).AsSplitQuery();

    public static Task<long> CountIfAsync<TEntity, TKey>(this IQueryable<TEntity> queryable,
        Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        where TKey : StronglyTypeId<Guid>
        where TEntity : BaseEntity<TKey>
    {
        var query = queryable.Where(predicate);
        query = query.AsSplitQuery();
        return query.LongCountAsync(cancellationToken);
    }

    public static async Task<IEnumerable<T>> GetFromQueryAsync<T>(
        this Microsoft.EntityFrameworkCore.DbContext dbContext, string sqlString,
        IEnumerable<object> parameters, CancellationToken cancellationToken = default) where T : notnull
    {
        if (string.IsNullOrEmpty(sqlString))
            throw new ArgumentNullException(nameof(sqlString));
        await using DbCommand command = dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandText = sqlString;
        IEnumerable<object> paramItems = parameters.ToList();
        foreach (var (item, index) in paramItems.Select((x, i) => (x, index: i)))
        {
            DbParameter dbParameter = command.CreateParameter();
            dbParameter.ParameterName = $@"@p{index}";
            dbParameter.Value = item;
            command.Parameters.Add(dbParameter);
        }

        try
        {
            await dbContext.Database.OpenConnectionAsync(cancellationToken);
            await using DbDataReader dbDataReader = await command.ExecuteReaderAsync(cancellationToken);

            List<T> results = new List<T>();
            while (await dbDataReader.ReadAsync(cancellationToken))
            {
                T item = Activator.CreateInstance<T>();
                if (!(typeof(T).IsPrimitive || typeof(T) == typeof(string)))
                {
                    item = (T)Convert.ChangeType(dbDataReader[0], typeof(T), CultureInfo.InvariantCulture);
                    results.Add(item);
                    continue;
                }

                foreach (PropertyInfo prop in item.GetType().GetProperties())
                {
                    string propertyName = prop.Name;
                    bool isExistedColumn = dbDataReader.IsExistedColumn(propertyName);
                    if (!isExistedColumn)
                        continue;

                    object value = dbDataReader[propertyName];
                    if (value == DBNull.Value)
                        continue;
                    prop.SetValue(item, value, null);
                }

                results.Add(item);
            }

            return results;
        }
        catch (System.Exception e)
        {
            throw new System.Exception(e.Message);
        }
        finally
        {
            await dbContext.Database.CloseConnectionAsync();
        }
    }

    public static async Task<IEnumerable<T>> GetFromQueryAsync<T>(
        this Microsoft.EntityFrameworkCore.DbContext dbContext, string sqlString,
        IEnumerable<DbParameter> parameters, CancellationToken cancellationToken = default) where T : notnull
    {
        if (string.IsNullOrEmpty(sqlString))
            throw new ArgumentNullException(nameof(sqlString));
        await using DbCommand command = dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandText = sqlString;

        var dbParameters = parameters as DbParameter[] ?? parameters.ToArray();
        if (!dbParameters.Any())
            command.Parameters.AddRange(dbParameters.ToArray());
        try
        {
            await dbContext.Database.OpenConnectionAsync(cancellationToken);
            await using DbDataReader dbDataReader = await command.ExecuteReaderAsync(cancellationToken);

            List<T> results = new List<T>();
            while (await dbDataReader.ReadAsync(cancellationToken))
            {
                T item = Activator.CreateInstance<T>();
                if (!(typeof(T).IsPrimitive || typeof(T) == typeof(string)))
                {
                    item = (T)Convert.ChangeType(dbDataReader[0], typeof(T), CultureInfo.InvariantCulture);
                    results.Add(item);
                    continue;
                }

                foreach (PropertyInfo prop in item.GetType().GetProperties())
                {
                    string propertyName = prop.Name;
                    bool isExistedColumn = dbDataReader.IsExistedColumn(propertyName);
                    if (!isExistedColumn)
                        continue;

                    object value = dbDataReader[propertyName];
                    if (value == DBNull.Value)
                        continue;
                    prop.SetValue(item, value, null);
                }

                results.Add(item);
            }

            return results;
        }
        catch (System.Exception e)
        {
            throw new System.Exception(e.Message);
        }
        finally
        {
            await dbContext.Database.CloseConnectionAsync();
        }
    }

    private static bool IsExistedColumn(this IDataReader reader, string columnName)
    {
        ArgumentNullException.ThrowIfNull(reader, nameof(reader));
        ArgumentNullException.ThrowIfNull(columnName, nameof(columnName));

        for (int i = 0; i < reader.FieldCount; i++)
        {
            if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}