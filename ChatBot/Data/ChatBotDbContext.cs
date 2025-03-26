using Microsoft.EntityFrameworkCore;
using ChatBot.Models;

namespace ChatBot.Data
{
    public class ChatBotDbContext : DbContext
    {
        public ChatBotDbContext(DbContextOptions<ChatBotDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
        }
    }
}
