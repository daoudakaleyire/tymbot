namespace tymbot.Models
{
    using System.ComponentModel.DataAnnotations;

    public class UserTimeZone
    {
        [Key]
        public int UserId { get; set; }

        [MaxLength(50)]
        public string TimeZone { get; set; }
    }
}