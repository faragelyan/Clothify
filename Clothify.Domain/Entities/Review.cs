namespace Clothify.Domain.Entities
{
    public class Review
    {
        public Guid ReviewId { get; set; }
        public byte Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
    }
}
