using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ToDoListApp.Models
{
    public class ToDoList
    {
        [Key]
        public int Id { get; set; }


        [Required]
        public string Task { get; set; }

        
        public bool IsCompleted { get; set; }

        
        public string UserName { get; set; }


        public string ListName { get; set; }

    }
}
