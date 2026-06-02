using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Convocatorias.WebApi.Controllers
{
    public class PostulacionController : Controller
    {
        // GET: PostulacionController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PostulacionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PostulacionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PostulacionController/Create
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

        // GET: PostulacionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PostulacionController/Edit/5
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

        // GET: PostulacionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PostulacionController/Delete/5
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
