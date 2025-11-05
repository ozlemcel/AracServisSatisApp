using Microsoft.AspNetCore.Identity;


namespace AracServisSatis.Entities.Concrete
{
    public class AppRole : IdentityRole<int>
    {
        public AppRole() : base() { } // EF Core'un istediği

        public AppRole(string roleName) : base(roleName) { } // manuel kullanım için

      
    }
    }

