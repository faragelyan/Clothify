namespace Clothify.Domain.Interfaces.Pagination
{
    public class PaginationResponse<T>
    {
        public List<T> Items { get; set; }
        public PaginationModel Pagination { get; set; }
    }
}
