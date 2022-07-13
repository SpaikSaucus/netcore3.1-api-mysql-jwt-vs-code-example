using System;
using System.Linq;
using Api.EFModels;

namespace Api.Services
{
  public class UserRepository : IUserRepository
  {
    private MyDbContext _context;

    public UserRepository(MyDbContext context)
    {
      _context = context;
    }

    public User Get(string guid)
    {
      return _context.Users.FirstOrDefault(x => x.guid == guid);
    }

    public User GetByUsername(string username)
    {
      return _context.Users.FirstOrDefault(x => x.username == username);
    }

    public User Create(User entity)
    {
      entity.created = DateTime.Now;
      _context.Users.Add(entity);
      _context.SaveChanges();

      return entity;
    }
  }
}