using System.Net.NetworkInformation;

namespace growgreen_backend.Models
{
    public class BlogModel
    {
        public int BlogID { get; set; }
        public int AdminID { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public DateTime PublishedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
