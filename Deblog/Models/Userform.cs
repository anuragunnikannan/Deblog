using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Deblog.Models
{
    public class Userform
    {
        [Key]
        [ValidateNever]
        public string Id { get; set; }

        [Required]
        [ValidateNever]
        public IFormFile Image { get; set; }

        [Required]
        public string Fullname { get; set; }

        [Required]
        public string UserDesc { get; set; }
    }
}
