using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Deblog.Models
{
    public class Blog
    {
        [Key]
        public int BlogId { get; set; }
        [Required]
        [DisplayName("Blog Title")]
        public string BlogTitle { get; set; }
        [Required]
        [ValidateNever]
        public string BlogBody { get; set; }
        [DisplayName("Image URL")]
        public string BlogImageURL { get; set; }
        [Required]
        [ValidateNever]
        public string BlogAuthor { get; set; }
        [Required]
        [DisplayName("Blog Type")]
        public string BlogType { get; set; }
        [Required]
        public DateTime BlogDateTime { get; set; }
        [DisplayName("Reading time")]
        public int BlogReadTime { get; set; }
        [Required]
        [DisplayName("Blog Topic")]
        public string BlogTopic { get; set; }
        [Required]
        [ValidateNever]
        [DisplayName("Blog Status")]
        public string BlogStatus { get; set; }
    }
}
