namespace growgreen_backend.Models
{
    public class PesticidesModel
    {
        public int PesticideID { get; set; }
        public string PesticidesName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public DateTime? ManufacturedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
