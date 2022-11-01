using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListApp.Data;
using ToDoListApp.Models;
using ToDoListApp.FileUploader;

namespace ToDoListApp.Controllers
{
    public class ToDoListController : Controller
    {
        private readonly ApplicationDBContext main_db;
        private readonly IFileUploader main_file;
        public string filePath;

        public ToDoListController(ApplicationDBContext db, IFileUploader file)
        {
            main_db = db;
            main_file = file;
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


        public IActionResult LoadListFromFile()
        { 

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoadListFromFile(IFormFile file)
        {
           GetFilePath(file);

            string[] lines = { };
            //string listFile = "C:\\Code\\Testing\\TestFile.txt";
            string listFile = filePath;
            //change to read name and state too
            lines = System.IO.File.ReadAllLines(listFile);

            for(int i = 0; i < lines.Length; i++)
            {
                var newTask = new ToDoList {Task =lines[i], Name = "", IsCompleted = false};

                main_db.Add(newTask);
                main_db.SaveChanges();

            }

            return RedirectToAction("Index");

        }

        public async void GetFilePath(IFormFile file)
        {
            if (file != null)
            {
               filePath = await main_file.UploadFile(file);
            }
        }

    }
}
