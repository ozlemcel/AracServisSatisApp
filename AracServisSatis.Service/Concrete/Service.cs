using AracServisSatis.Data;
using AracServisSatis.Data.Concrete;
using AracServisSatis.Entities.Abstract;
using AracServisSatis.Service.Abstract;

namespace AracServisSatis.Service.Concrete
{
    public class Service<T> : Repository<T>, IService<T> where T : class, IEntity, new()
    {
        public Service(DatabaseContext context) : base(context)
        {
        }

       
    }
}

