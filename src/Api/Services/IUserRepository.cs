using Api.EFModels;

namespace Api.Services
{
  public interface IUserRepository
  {
    User Get(string guid);
    User GetByUsername(string username);
    User Create(User entity);
  }
}