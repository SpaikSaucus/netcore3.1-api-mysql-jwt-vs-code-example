using Microsoft.EntityFrameworkCore;

namespace Api.EFModels
{
  public partial class MyDbContext : DbContext
  {
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    { }
    public virtual DbSet<User> Users { get; set; }
  }
}