using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Deblog.Models
{
    public class Userdata
    {
        [Key]
        [ValidateNever]
        public string Id { get; set; }

        [Required]
        public string Username { get; set; }

        [ValidateNever]
        public string ImageURL { get; set; }
        [Required]
        public string Fullname { get; set; }
        [Required]
        public string UserDesc { get; set; }
        public DateTime DOJ { get; set; }

    }
}
