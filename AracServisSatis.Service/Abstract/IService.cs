using AracServisSatis.Data.Abstract;
using AracServisSatis.Entities.Abstract;

namespace AracServisSatis.Service.Abstract
{
    public interface IService<T> : IRepository<T> where T : class, IEntity, new()
    {
   
    }
}
