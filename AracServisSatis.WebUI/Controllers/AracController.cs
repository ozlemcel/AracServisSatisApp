using AracServisSatis.Entities.Concrete;
using AracServisSatis.Service.Abstract;
using Microsoft.AspNetCore.Mvc;


namespace AracServisSatis.WebUI.Controllers
{
    public class AracController : Controller
    {
        private readonly ICarService _serviceArac;
        private readonly IService<Musteri> _serviceMusteri;

        public AracController(ICarService serviceArac, IService<Musteri> serviceMusteri)
        {
            _serviceArac = serviceArac;
            _serviceMusteri = serviceMusteri;
        }

        public async Task<IActionResult> IndexAsync(int id)
        {
            var model = await _serviceArac.GetCustomCar(id);
            return View(model);
        }

        [Route("tum-araclarimiz")]
        public async Task<IActionResult> ListAsync()
        {
            var model = await _serviceArac.GetCustomCarList(c => c.SatistaMi);
            return View(model);
        }
        public async Task<IActionResult> Ara(string q)
        {
            var model = await _serviceArac.GetCustomCarList(c => c.SatistaMi && c.Marka.Adi.Contains(q) || c.KasaTipi.Contains(q) || c.Modeli.Contains(q));
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> MusteriKayit(Musteri musteri)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceMusteri.AddAsync(musteri);
                    await _serviceMusteri.SaveAsync();
                    // await MailHelper.SendMailAsync(musteri);
                    TempData["Message"] = "<div class='alert alert-success'>Talebiniz Alınmıştır. Teşekkürler..</div>";
                    return Redirect("/Arac/Index/" + musteri.AracId);
                }
                catch
                {
                    TempData["Message"] = "<div class='alert alert-danger'>Bir Hata Oluştu!</div>";
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View();
        }
    }
}
