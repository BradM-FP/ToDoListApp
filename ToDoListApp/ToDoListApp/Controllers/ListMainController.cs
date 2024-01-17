using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using ToDoListApp.Data;
using ToDoListApp.Models;


namespace ToDoListApp.Controllers
{
    //This controller manages the main list names and who they are saved to, not the detail of the lists.
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
                //Testing
                obj.UserName = User.Identity.Name;
            }
            else
            {
                obj.UserName = "Guest";
            }

            if (ModelState.IsValid)
            {
                main_db.ListM.Add(obj);
                main_db.SaveChanges();
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "ToDoList", action = "Index", currentL = obj.ListName }));
            }

            return View();
        }

        public IActionResult Edit(int? id, string currentListName)
        {
            TempData["CurrentListName"] = currentListName;

            if (id == null || id == 0)
            {
                return NotFound();
            }

            var taskFromDb = main_db.ListM.Find(id);


            if (taskFromDb == null)
            {
                return NotFound();
            }

            return View(taskFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ListMain obj, string currentListName)
        {
            TempData["CurrentListName"] = currentListName;
            ////Asign owner of list
            if (User.Identity.IsAuthenticated)
            {
                obj.UserName = User.Identity.Name;
            }
            else
            {
                obj.UserName = "Guest";
            }

            if (ModelState.IsValid)
            {
                main_db.ListM.Update(obj);
                main_db.SaveChanges();

                return RedirectToAction("UpdateListName", new RouteValueDictionary(new { controller = "ToDoList", action = "UpdateListName", newListName = obj.ListName, currentListName = currentListName }));
            }

            return View();

        }

        public IActionResult LoadTemplate()
        {

            List<string> templates = new List<string>();

            string[] tempList = Directory.GetFiles("wwwroot/Templates");

            foreach(string str in tempList)
            {
                templates.Add(str);
            }
            

            //Checking if user is signed in, then if they have any custom templates
            if(User.Identity.IsAuthenticated)
            {
                if (Directory.Exists("wwwroot/Templates/" + User.Identity.Name))
                {
                    string[] userTemplates = Directory.GetFiles("wwwroot/Templates/" + User.Identity.Name);

                    foreach(string str in userTemplates)
                    {
                        templates.Add(str);
                    }
               
                }
                
            }

            List<ListMain> files = new List<ListMain>();

            //Renaming read files to remove .txt and adding them to a list to return
            foreach (string filePath in templates)
            {
                string fileWithoutTxt = filePath;
                fileWithoutTxt = fileWithoutTxt.Replace(".txt", "");
                files.Add(new ListMain { ListName = Path.GetFileName(fileWithoutTxt) });
            }

            return View(files);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoadTemplate(string templateName)
        {
            return View();
        }

        public IActionResult SaveTemplate(string templateName)
        {
            TempData["TemplateName"] = templateName;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveTemplate(string templateName, string listName) 
        {
            ListMain listMain = new ListMain();

            if(User.Identity.IsAuthenticated)
            {
                listMain.UserName = User.Identity.Name;
            }
            else
            {
                listMain.UserName = "Template";
            }
            listMain.ListName = listName;


            main_db.ListM.Add(listMain);
            main_db.SaveChanges();

            return RedirectToAction("SaveTemplate", new RouteValueDictionary(new { controller = "ToDoList", action = "SaveTemplate", templateName = templateName, listName = listName  }));
        }

        public async Task<IActionResult> DeleteList(string listName)
        {

            var listToDelete = await main_db.ListM.AsNoTracking().Where(x => x.ListName == listName).FirstOrDefaultAsync();

            if(listToDelete == null)
            {
                return View("Error");
            }

            main_db.ListM.Remove(listToDelete);
            main_db.SaveChanges();

            return RedirectToAction("DeleteList", new RouteValueDictionary(new { controller = "ToDoList", action = "DeleteList", listName = listName }));
        }

        public IActionResult SaveAsTemplate(string templateName)
        {
            TempData["TemplateName"] = templateName;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveAsTemplate(ListMain obj, string templateName)
        {
            return RedirectToAction("SaveAsTemplate", new RouteValueDictionary(new { controller = "ToDoList", action = "SaveAsTemplate", listName = obj.ListName, templateName = templateName }));
        }

    }
}
