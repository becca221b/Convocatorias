using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Application.UseCases.Postulaciones.Get;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Convocatorias.WebApi.Controllers
{
    public class PostulacionController : Controller
    {
        private readonly GetAll _getAllUseCase;
        private readonly GetById _getByIdUse;
        private readonly GetPostulacionesByConvocatoriaId _getByConvocatoriaIdUseCase;

        public PostulacionController(GetAll getAllUseCase, GetById getByIdUse, GetPostulacionesByConvocatoriaId getByConvocatoriaIdUseCase)
        {
            _getAllUseCase = getAllUseCase;
            _getByIdUse = getByIdUse;
            _getByConvocatoriaIdUseCase = getByConvocatoriaIdUseCase;
        }

        [HttpGet]
        // GET: PostulacionController
        public async Task<ActionResult> GetAll(CancellationToken ct)
        {
            var postulaciones = await _getAllUseCase.ExecuteAsync(ct);
            return View(postulaciones);
        }

        // GET: PostulacionController/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            var postulacion = await _getByIdUse.ExecuteAsync(id, CancellationToken.None);
            if (postulacion == null)
            {
                return NotFound();
            }
            return View(postulacion);
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
