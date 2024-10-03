namespace Core.DTOs
{
    public class PaginacionResponse<T>
    {
        public List<T> Items { get; set; } // Lista de elementos de tipo T
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }
}
