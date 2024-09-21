namespace backend.Dto.Common
{
    public class PageResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
    }
}