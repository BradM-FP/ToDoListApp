using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ToDoListApp.Areas.Identity.Data;
using ToDoListApp.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Data.SqlClient;

namespace ToDoListApp.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            
        }
        //This creates the table with the columns inside the model
        public DbSet<ToDoList> ToDo { get; set; }

        public DbSet<ListMain> ListM { get; set; }


    }


}
