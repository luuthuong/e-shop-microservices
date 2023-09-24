using Newtonsoft.Json.Linq;

namespace Infrastructure.GraphQL;

public class GraphQLQuery
{
    public string OperationName { get; set; }
    public string NameQuery { get; set; }
    public string Query { get; set; }
    public JObject Variables { get; set; }
    
}