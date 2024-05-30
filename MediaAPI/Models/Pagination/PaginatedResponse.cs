namespace MediaAPI.Models.Pagination
{
    public class PaginatedResponse<T>
    {
        public List<T>? ResponseData { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
    }

}
