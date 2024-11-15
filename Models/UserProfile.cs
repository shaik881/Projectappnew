using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DefaultIdentityColumnRename.Models
{
    public class UserProfile
    {
        [Key]
        public int ProfileId { get; set; }

        // Foreign Key to AspNetUsers table
       
        [ForeignKey("User")]
        public string UserId { get; set; }

        // Navigation property to link to the ApplicationUser
        public virtual User User { get; set; }

        public byte[] Pic { get; set; }

      
        [MaxLength(500)]
        public string? Address { get; set; }

      
        [MaxLength(10)]
        public string? Pincode { get; set; }

        [MaxLength(100)]
        public string? State { get; set; }
        public string? Country {  get; set; }
        public string? Gender {  get; set; }
    }
}
