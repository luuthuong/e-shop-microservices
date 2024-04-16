using Core.Models;

namespace API.Requests.Products;

internal sealed record ProductGetListRequest(
    int PageSize,
    int PageIndex,
    string Keyword,
    string OrderBy = "",
    bool Descending = false) : PageRequest(PageSize, PageIndex, OrderBy, Descending);