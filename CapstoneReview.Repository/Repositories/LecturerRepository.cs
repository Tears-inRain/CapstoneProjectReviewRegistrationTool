using System.Collections.Generic;
using System.Threading.Tasks;
using CapstoneReview.Repository.Data.DbContexts;
using CapstoneReview.Repository.Entities;
using CapstoneReview.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CapstoneReview.Repository.Repositories;

public class LecturerRepository : ILecturerRepository
{
    private readonly ApplicationDbContext _context;

    public LecturerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Lecturer>> GetAllLecturersWithDetailsAsync()
    {
        return await _context.Lecturers
            .Include(l => l.SlotLecturers)
            .ToListAsync();
    }

    public Task<Lecturer?> GetByIdAsync(int id)
    {
        return _context.Lecturers.FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<Lecturer?> GetLecturerByIdWithDetailsAsync(int lecturerId)
    {
        return await _context.Lecturers
            .Include(l => l.SlotLecturers)
            .FirstOrDefaultAsync(l => l.Id == lecturerId);
    }
}
