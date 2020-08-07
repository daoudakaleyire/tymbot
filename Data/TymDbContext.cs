namespace tymbot.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using tymbot.Models;

    public class TymDbContext : DbContext
    {
        public TymDbContext(DbContextOptions<TymDbContext> options) : base(options)
        {
        }
            
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<UserFriend>().ToTable("UserFriend");
            
            EntityTypeBuilder<UserFriend> userFriendEntity = modelBuilder.Entity<UserFriend>();
            userFriendEntity.HasKey(tr => new { tr.UserId, tr.FriendId });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserFriend> UserFriends { get; set; }
    }
}