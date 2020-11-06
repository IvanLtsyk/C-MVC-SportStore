using System;

namespace SportStorage.Models.ViewModels
{
    public class PaginationInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPages { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages => (int) Math.Ceiling((decimal) TotalItems / ItemsPerPages);
    }
}