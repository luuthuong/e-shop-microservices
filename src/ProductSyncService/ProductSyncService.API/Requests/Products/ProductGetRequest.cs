using System.Reflection;
using Core.Models;

namespace API.Requests.Products;

public record ProductGetRequest(int PageSize, int PageIndex, string Keyword, string OrderBy = "", bool Descending = false) : PageRequest(PageSize, PageIndex, OrderBy, Descending);