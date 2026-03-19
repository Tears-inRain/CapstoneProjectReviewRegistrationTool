using System.Threading.Tasks;
using CapstoneReview.Repository.Data.DbContexts;
using CapstoneReview.Repository.Interfaces;

namespace CapstoneReview.Repository.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public ISlotRepository Slots { get; private set; }
    public ILecturerRepository Lecturers { get; private set; }
    public ITeamRepository Teams { get; private set; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Slots = new SlotRepository(_context);
        Lecturers = new LecturerRepository(_context);
        Teams = new TeamRepository(_context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
