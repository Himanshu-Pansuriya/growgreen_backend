namespace growgreen_backend.Models
{
    public class CropsTransactionModel
    {
        public int CropsTransactionID { get; set; }
        public int BuyerID { get; set; }
        public int CropID { get; set; }
        public decimal QuantityPurchased { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
