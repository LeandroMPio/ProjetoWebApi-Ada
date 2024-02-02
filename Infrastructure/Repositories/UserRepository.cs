using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;


public interface IUserRepository
{
    Task<List<User>> List();
    Task<User?> GetById(int id);
    Task<User?> FindByEmail(string email);
    Task<User> Create(User newUser);
    Task<User> Update(User updateUser);
    Task Delete(int id);
}
public class UserRepository : IUserRepository
{
    private readonly Context _context;

    public UserRepository(Context context)
    {
        _context = context;
    }

    public async Task<User> Create(User newUser)
    {
        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
        return newUser;

    }

    public async Task<List<User>> List()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetById(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<User?> FindByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<User> Update(User updateUser)
    {
        var user = await GetById(updateUser.Id);

        if (user is null) throw new Exception("User not found");

        user.Name = updateUser.Name;
        user.Email = updateUser.Email;
        user.Password = updateUser.Password;
        user.Role = updateUser.Role;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task Delete(int id)
    {
        var user = await GetById(id);

        if (user is null) throw new Exception("User not found!");

        _context.Users.Remove(user);

        await _context.SaveChangesAsync();
    }
}
