namespace tymbot.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class UserFriend
    {
        public long UserId { get; set; }

        public long FriendId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("FriendId")]
        public User Friend { get; set; }
    }
}