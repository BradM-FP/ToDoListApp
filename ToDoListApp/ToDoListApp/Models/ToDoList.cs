using System.ComponentModel.DataAnnotations;

namespace ToDoListApp.Models
{
    public class ToDoList
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Task { get; set; }

        [Required]
        public bool IsCompleted { get; set; }

    }
}
