using System.Collections.Generic;

namespace FiapConnect.DTOs
{
    public class PagedResultDTO<T>
    {
        public List<T> Data { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }

        // Links para navegação
        public Dictionary<string, string> Links { get; set; }
    }
}