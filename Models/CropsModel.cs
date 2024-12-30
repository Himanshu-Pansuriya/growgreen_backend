namespace growgreen_backend.Models
{
    public class CropsModel
    {
        public int CropID { get; set; }
        public int FarmerID { get; set; }
        public string CropName { get; set; }
        public string CropType { get; set; }
        public decimal Quantity { get; set; }
        public decimal PricePer20KG { get; set; }
        public string Description {  get; set; }
        public string status { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }

    }
}
