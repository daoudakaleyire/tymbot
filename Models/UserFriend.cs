namespace tymbot.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class UserFriend
    {
        public int UserId { get; set; }

        public int FriendId { get; set; }

        [ForeignKey("UserId")]
        public UserTimeZone UserTimeZone { get; set; }
    }
}