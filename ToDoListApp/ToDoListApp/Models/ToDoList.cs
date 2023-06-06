using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ToDoListApp.Models
{
    public class ToDoList
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "Please Enter Name of Task")]
        public string Task { get; set; }

        
        public bool IsCompleted { get; set; }

        
        public string? UserName { get; set; }


        public string? ListName { get; set; }

        public bool ImportantTask { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyy HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime? FinishByDate { get; set; }   

    }
}
