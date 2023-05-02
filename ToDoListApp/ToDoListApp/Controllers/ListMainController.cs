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
        public ActionResult Index()
        {
            IEnumerable<ListMain> objToDoList = main_db.ListM;

            return View(objToDoList);
        }

        // GET: ListMainController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ListMainController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ListMainController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ListMainController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ListMainController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ListMainController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ListMainController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        

    }
}
