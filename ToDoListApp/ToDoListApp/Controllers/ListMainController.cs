using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoListApp.Data;
using ToDoListApp.Models;

namespace ToDoListApp.Controllers
{
    public class ListMainController : Controller
    {
        private readonly ApplicationDBContext main_db;

        public ListMainController(ApplicationDBContext db)
        {
            main_db = db;
        }

        // GET: ListMainController
        public IActionResult Index()
        {
            IEnumerable<ListMain> objToDoList = main_db.ListM;

            return View(objToDoList);
        }


        public IActionResult AddNewList()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddNewList(ListMain obj)
        {
            if (User.Identity.IsAuthenticated)
            {
                obj.UserName = User.Identity.Name;
            }
            else
            {
                obj.UserName = "Guest";
            }

            main_db.ListM.Add(obj);
            main_db.SaveChanges();
            return View(obj);
        }


    }
}
