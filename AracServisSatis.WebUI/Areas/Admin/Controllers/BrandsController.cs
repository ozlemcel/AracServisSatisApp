using AracServisSatis.Entities.Concrete;
using AracServisSatis.Service.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AracServisSatis.WebUI.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles="Admin")]
    public class BrandsController : Controller
    {
        private readonly IService<Marka> _service;

        public BrandsController(IService<Marka> service)
        {
            _service = service;
        }

        // GET: BrandsController
        public async Task<ActionResult> Index()
        {
            var model = await _service.GetAllAsync();
            return View(model);
        }



        // GET: BrandsController/Details
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BrandsController/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: BrandsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Marka marka)
        {
            try
            {
                await _service.AddAsync(marka);
                await _service.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Hata Oluştu!");
            }
            return View(marka);
        }


        // GET: BrandsController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            var model = await _service.FindAsync(id);
            return View(model);
        }

        // POST: BrandsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, Marka marka)
        {
            try
            {
                _service.Update(marka);
                await _service.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                
            }
            return View(marka);
        }

        // GET: BrandsController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var model = await _service.FindAsync(id);
            return View(model);
        }

        // POST: BrandsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, Marka marka)
        {
            try
            {
                _service.Delete(marka);
                await _service.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(marka);
            }
        }
    }
}
