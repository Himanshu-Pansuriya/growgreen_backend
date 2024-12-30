namespace growgreen_backend.Models
{
    public class PesticidesTransactionModel
    {
        public int PesticidesTransactionID { get; set; }
        public int BuyerID { get; set; }
        public int PesticideID { get; set; }
        public int QuantityPurchased { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
