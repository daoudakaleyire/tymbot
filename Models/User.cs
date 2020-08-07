namespace tymbot.Models
{
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        [Key]
        public long UserId { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string TimeZone { get; set; }

        public long? ChatId { get; set; }
    }
}