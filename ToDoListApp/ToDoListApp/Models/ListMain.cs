using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

    namespace ToDoListApp.Models
{
    public class ListMain
    {

        [Key]
        public int Id { get; set; }


        public string? UserName { get; set; }

        [Required(ErrorMessage = "Please Enter Name of the List")]
        public string ListName { get; set; }


    }
}
