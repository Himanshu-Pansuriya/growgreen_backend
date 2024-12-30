namespace growgreen_backend.Models
{
    public class PesticidesConfirmModel
    {
        public int PesticidesConfirmID { get; set; }
        public int PesticidesTransactionID { get; set; }
        public int SalesmanID { get; set; }
        public string Status { get; set; }
    }
}
