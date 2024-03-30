namespace Core.Models;

public record PageRequest(int PageSize, int PageIndex, string OrderBy = "", bool Descending = false);