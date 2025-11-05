using AracServisSatis.Data;
using AracServisSatis.Data.Concrete;
using AracServisSatis.Service.Abstract;


namespace AracServisSatis.Service.Concrete
{
    public class CarService : CarRepository, ICarService
    {
        public CarService(DatabaseContext context) : base(context)
        {
        }
    }
}
