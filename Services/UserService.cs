using System;
using RAAA.DataModel;

namespace RAAA.Services
{
    public interface IUserService
    {
        internal void LoadUser();

        public User GetCurrentUser();
    }
    public class UserService : IUserService
    {
        public User GetCurrentUser()
        {
            throw new NotImplementedException();
        }

        User IUserService.GetCurrentUser()
        {
            throw new NotImplementedException();
        }

        void IUserService.LoadUser()
        {
            throw new NotImplementedException();
        }
    }
}
