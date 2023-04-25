using Microsoft.AspNetCore.Mvc;
using ToDoListApp.Data;
using ToDoListApp.Models;

namespace ToDoListApp.Controllers
{
    public class ToDoListController : Controller
    {
        private readonly ApplicationDBContext main_db;

        public ToDoListController(ApplicationDBContext db)
        {
            main_db = db;   
        }

        public IActionResult Index()
        {
            IEnumerable<ToDoList> objToDoList = main_db.ToDo;

            return View(objToDoList);
        }

        public IActionResult AddNewTask()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddNewTask(ToDoList obj)
        {

            if(ModelState.IsValid)
            {
                main_db.ToDo.Add(obj);
                main_db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var taskFromDb = main_db.ToDo.Find(id);


            if (taskFromDb == null)
            {
                return NotFound();
            }

            return View(taskFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ToDoList obj)
        {
            if (ModelState.IsValid)
            {
                main_db.ToDo.Update(obj);
                main_db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var taskFromDb = main_db.ToDo.Find(id);

            if (taskFromDb == null)
            {
                return NotFound();
            }

            return View(taskFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = main_db.ToDo.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            main_db.ToDo.Remove(obj);
            main_db.SaveChanges();
            return RedirectToAction("Index");

        }

        public IActionResult CompleteTask(int? id)
        {

            if (id == null || id == 0)
            {
                return NotFound();
            }

            var taskFromDb = main_db.ToDo.Find(id);

            if (taskFromDb == null)
            {
                return NotFound();
            }

            if (taskFromDb.IsCompleted)
            {
                taskFromDb.IsCompleted = false;
            }
            else
            {
                taskFromDb.IsCompleted = true;
            }

            if (ModelState.IsValid)
            {
                main_db.ToDo.Update(taskFromDb);
                main_db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public IActionResult LoadTemplate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoadTemplate(ToDoList obj)
        {
       

            return View(obj);
        }

    }
}
