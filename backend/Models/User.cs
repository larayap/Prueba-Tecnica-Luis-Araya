using System.ComponentModel.DataAnnotations;

namespace Ruleta.Models
{
    public class User
    {
        [Key]
        public string Name { get; set; } = string.Empty;
        public int Balance { get; set; }
    }
}
