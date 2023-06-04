using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
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
        public IActionResult AddNewList(ListMain obj, string currentL)
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
            return RedirectToAction("Index", new RouteValueDictionary(new { controller = "ToDoList", action ="Index", currentL = obj.ListName }));
        }

        public IActionResult LoadTemplate()
        {

            string[] templates = Directory.GetFiles("wwwroot/Templates");

            List<ListMain> files = new List<ListMain>();


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

    }
}
