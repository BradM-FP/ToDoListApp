using Microsoft.EntityFrameworkCore;
using ToDoListApp.Models;


namespace ToDoListApp.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
      
        }

        //This creates the table with the columns inside the model

        public DbSet<ToDoList> ToDo { get; set; }

    }
}
