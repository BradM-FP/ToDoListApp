using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public IActionResult AddNewTask(string currentL)
        {
            TempData["CurrentList"] = currentL;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddNewTask(ToDoList obj, string currentL)
        {
            if(User.Identity.IsAuthenticated)
            {
                obj.UserName = User.Identity.Name;
            }
            else
            {
                obj.UserName = "Guest";
            }
            
            obj.ListName = currentL;

            main_db.ToDo.Add(obj);
            main_db.SaveChanges();

            return LoadList(currentL);

        }

        public IActionResult Edit(int? id, string currentL)
        {
            TempData["CurrentList"] = currentL;

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
        public IActionResult Edit(ToDoList obj, string currentL)
        {
           obj.ListName= currentL;

            if (User.Identity.IsAuthenticated)
            {
                obj.UserName = User.Identity.Name;
            }
            else
            {
                obj.UserName = "Guest";
            }

           main_db.ToDo.Update(obj);
           main_db.SaveChanges();

            return LoadList(currentL);

        }

        public IActionResult Delete(int? id, string currentL)
        {
            TempData["CurrentList"] = currentL;

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
        public IActionResult DeletePost(int? id, string currentL)
        {
            var obj = main_db.ToDo.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            main_db.ToDo.Remove(obj);
            main_db.SaveChanges();
            return LoadList(currentL);

        }

        public IActionResult CompleteTask(int? id, string currentL)
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

                IEnumerable<ToDoList> objToDoList = main_db.ToDo;

                return LoadList(currentL);

            }
            else
            {
                return LoadList(currentL);
            }

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


        public IActionResult LoadList(string? name)
        {
            IEnumerable<ToDoList> objToDoList = main_db.ToDo;

            return View("Index", GetList(objToDoList, name));
        }

        public List<ToDoList> GetList(IEnumerable<ToDoList> objToDoList, string name )
        {
            List<ToDoList> tdList = new List<ToDoList>();

            tdList = objToDoList.ToList();

            foreach (ToDoList item in tdList.ToList())
            {
                if (item.ListName == name && item.UserName == User.Identity.Name)
                {
                    continue;
                }
                else
                {
                    tdList.Remove(item);
                }
            }

            return tdList;
        }


    }
}
