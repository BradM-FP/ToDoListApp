using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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

        public IActionResult Index(string currentL, int tableType = 0)
        {
            TempData["CurrentList"] = currentL;

            return LoadList(currentL, tableType);
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
            TempData["CurrentList"] = currentL;

            //Assign name of list the task will be saved to.
            obj.ListName = currentL;

            //Asign owner of new task
            if (User.Identity.IsAuthenticated)
            {
                obj.UserName = User.Identity.Name;
            }
            else
            {
                obj.UserName = "Guest";
            }

            //If no date has been selected, assign it the default value
            if (obj.FinishByDate is null)
            {
                obj.FinishByDate = DateTime.Parse("0001-01-01 00:00:00.0000000");
            }


            if (ModelState.IsValid)
            {
                main_db.ToDo.Add(obj);
                main_db.SaveChanges();

                return LoadList(currentL);
            }
            else
            {
                return View();
            }
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
            TempData["CurrentList"] = currentL;

            obj.ListName = currentL;
            ////Asign owner of new task
            if (User.Identity.IsAuthenticated)
            {
                obj.UserName = User.Identity.Name;
            }
            else
            {
                obj.UserName = "Guest";
            }
            //If no date has been selected, assign it the default value
            if (obj.FinishByDate is null)
            {
                obj.FinishByDate = DateTime.Parse("0001-01-01 00:00:00.0000000");
            }

            if (ModelState.IsValid)
            {
                main_db.ToDo.Update(obj);
                main_db.SaveChanges();

                return LoadList(currentL);
            }

            return View();

        }

        public IActionResult Delete(int? id, string currentL)
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

        public IActionResult DeleteList(string listName)
        {
            //Go through and delete all items from list.
            foreach (ToDoList obj in main_db.ToDo)
            {
                if (obj.ListName == listName && obj.UserName == User.Identity.Name)
                {
                    main_db.ToDo.Remove(obj);
                }
            }
            main_db.SaveChanges();
            return RedirectToAction("Index", "Home", null);
        }

        public IActionResult CompleteTask(int? id, string currentL)
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

                return LoadList(currentL);

            }
            else
            {
                return LoadList(currentL);
            }

        }

        public IActionResult LoadTemplate(string templateName)
        {
            TempData["TemplateName"] = templateName;

            return View("LoadTemplate", GetListFromTemplate(templateName));
        }

        public IActionResult SaveTemplate(string templateName, string listName)
        {
            List<ToDoList> tdList = new List<ToDoList>();

            tdList = GetListFromTemplate(templateName);

            foreach (ToDoList obj in tdList)
            {
                obj.ListName = listName;
                main_db.Add(obj);
                main_db.SaveChanges();
            }

            return LoadList(tdList[0].ListName);
        }

        public IActionResult SaveAsTemplate(string templateName, string listName)
        {
            string filepath;
            //If user is signed in, check if they already have a custom directory for templates, if not, create one
            if (User.Identity.IsAuthenticated)
            {
                filepath = "wwwroot/Templates/" + User.Identity.Name;

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                filepath = filepath + "/" + listName + ".txt";
            }
            else
            {
                filepath = "wwwroot/Templates/" + listName + ".txt";

            }

            //Write the tasks to the new template text file
            using (StreamWriter writer = System.IO.File.CreateText(filepath))
            {
                foreach (ToDoList obj in main_db.ToDo)
                {
                    if (obj.ListName == templateName && User.Identity.Name == obj.UserName)
                    {
                        writer.WriteLine(obj.Task);
                    }

                }
            }

            return RedirectToAction("Index", "Home", null);
        }

        public IActionResult LoadList(string? name, int tableType = 0)
        {
            TempData["CurrentList"] = name;

            IEnumerable<ToDoList> objToDoList = main_db.ToDo;

            return View("Index", GetList(objToDoList, name, tableType));


        }

        public List<ToDoList> GetList(IEnumerable<ToDoList> objToDoList, string name, int tableType = 0)
        {
            List<ToDoList> tdList = new List<ToDoList>();

            tdList = objToDoList.ToList();

            string nameToSave;

            if (User.Identity.IsAuthenticated)
            {
                nameToSave = User.Identity.Name;
            }
            else
            {
                nameToSave = "Guest";
            }


            //Getting accurate list of tasks depending on list chosen. Removes them from list if they don't belong to the user. Options in switch are for different filters. 1 = imporant, 2 = overdue
            foreach (ToDoList item in tdList.ToList())
            {
                if (item.ListName == name && item.UserName == nameToSave)
                {
                    switch (tableType)
                    {
                        case 0:
                            continue;
                        case 1:
                            if (!item.ImportantTask)
                            {
                                tdList.Remove(item);
                                continue;
                            }
                            else
                            {
                                continue;
                            }
                        case 2:
                            if (item.FinishByDate != null)
                            {
                                //If task is not completed and task is past it's due by date, show it
                                if (DateTime.Compare((DateTime)item.FinishByDate, DateTime.Today) <= 0 && item.FinishByDate != DateTime.Parse("0001-01-01 00:00:00.0000000") && !item.IsCompleted)
                                {
                                    continue;
                                }
                                else
                                {
                                    tdList.Remove(item);
                                    continue;
                                }
                            }
                            else
                            {
                                tdList.Remove(item);
                                continue;
                            }

                        default:
                            break;
                    }

                    continue;
                }
                else
                {
                    tdList.Remove(item);
                }
            }
            return tdList;
        }

        public List<ToDoList> GetListFromTemplate(string templateName)
        {

            string filepath;
            //Checking the directory for the template chosen exists, if not it's probably a custom list so check the users folder.
            if (System.IO.File.Exists("wwwroot/Templates/" + templateName + ".txt"))
            {
                filepath = "wwwroot/Templates/" + templateName + ".txt";
            }
            else
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (Directory.Exists("wwwroot/Templates/" + User.Identity.Name))
                    {
                        filepath = "wwwroot/Templates/" + User.Identity.Name + "/" + templateName + ".txt";
                    }
                    else
                    {
                        filepath = "No List Found";
                    }
                }
                else
                {
                    filepath = "No List Found";
                }
            }
            

            var lines = System.IO.File.ReadAllLines(filepath);

            List<ToDoList> tdList = new List<ToDoList>();
            //Write the template to a list so that it can be displayed to the user.
            foreach (string line in lines)
            {
                ToDoList obj = new ToDoList();
                obj.Task = line;
                obj.ListName = templateName;
                if (User.Identity.IsAuthenticated)
                {
                    obj.UserName = User.Identity.Name;
                }
                else
                {
                    obj.UserName = "guest";
                }

                tdList.Add(obj);
            }

            return tdList;
        }

        public IActionResult UpdateListName(string newListName, string currentListName)
        {
            //If the listname in the main has been updated, loop through and update it here too
            foreach(ToDoList obj in main_db.ToDo)
            {
                if(obj.ListName == currentListName && obj.UserName == User.Identity.Name) 
                {
                    obj.ListName = newListName;
                    main_db.Update(obj);
                    
                }
            }
            main_db.SaveChanges();
            return RedirectToAction("Index", "ListMain", null);
        }


    }
}
