using System.ComponentModel.DataAnnotations;

namespace Deblog.Models
{
	public class Bookmark
	{
		[Required]
		public string Id { get; set; }

		[Required]
		public int BlogId { get; set; }
	}
}
