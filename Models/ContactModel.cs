namespace growgreen_backend.Models
{
    public class ContactModel
    {
        public int ContactID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string? Email { get; set; }
        public string? Description { get; set; }
    }
}
