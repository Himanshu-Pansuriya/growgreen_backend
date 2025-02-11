using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace growgreen_backend.Models
{
    public class PesticidesModel
    {
        [Key]
        public int? PesticideID { get; set; }
        public string PesticidesName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public DateTime? ManufacturedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string ImageUrl { get; set; } // URL stored in DB

        [NotMapped]
        public IFormFile? ImageFile { get; set; } // For upload only
    }

    public class ImageUploadModel
    {
        public IFormFile File { get; set; }
    }
    public class ImageEntity
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public DateTime UploadedAt { get; set; }
    }
    public class PesticidesUploadModel
    {
        public int PesticideID { get; set; }
        public string PesticidesName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public DateTime? ManufacturedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public IFormFile? File { get; set; }  
    }

}
