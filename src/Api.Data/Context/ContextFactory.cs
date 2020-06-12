using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Api.Data.Context
{
  public class ContextFactory : IDesignTimeDbContextFactory<MyContext>
  {
    public MyContext CreateDbContext(string[] args)
    {
      // Using to create the Migrations
      //var connectionString = "Server=localhost;Port=3306;Database=dbApi;Uid=root;Pwd=dbApi@123";
      var connectionString = "Server=.\\SQLEXPRESS;Database=dbApi;User Id=sa;Pwd=dbApi@123";
      var optionsBuilder = new DbContextOptionsBuilder<MyContext> ();
      //optionsBuilder.UseMySql (connectionString);
      optionsBuilder.UseSqlServer (connectionString);
      return new MyContext (optionsBuilder.Options);
    }
  }
}
