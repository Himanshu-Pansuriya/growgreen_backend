using System.ComponentModel.DataAnnotations.Schema;

namespace growgreen_backend.Models
{
    public class CropsModel
    {
        public int CropID { get; set; }
        public int FarmerID { get; set; }
        public string UserName { get; set; }
        public string CropName { get; set; }
        public string CropType { get; set; }
        public decimal? Quantity { get; set; }
        public decimal PricePer20KG { get; set; }
        public string? Description {  get; set; }
        public string status { get; set; }
        public string ContactNo { get; set; }
        public string? Address { get; set; }
        public string? ImageUrl {  get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; } // For upload only
    }

    public class CropsUploadModel
    {
        public int CropID { get; set; }
        public int FarmerID { get; set; }
        public string CropName { get; set; }
        public string CropType { get; set; }
        public int? Quantity { get; set; }
        public decimal PricePer20KG { get; set; }
        public string Description { get; set; }
        public string status { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public IFormFile? File { get; set; } // For image upload
        public string? ImageUrl { get; set; } // Optional: Store existing image URL in case of update
    }

}
