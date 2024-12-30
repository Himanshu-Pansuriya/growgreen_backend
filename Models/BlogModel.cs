namespace growgreen_backend.Models
{
    public class BlogModel
    {
        public int BlogID { get; set; }
        public int AdminID { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}
