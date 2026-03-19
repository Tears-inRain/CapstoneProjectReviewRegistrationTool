using System.Collections.Generic;
using System.Threading.Tasks;
using CapstoneReview.Repository.Entities;

namespace CapstoneReview.Repository.Interfaces;

public interface ILecturerRepository
{
    Task<List<Lecturer>> GetAllLecturersWithDetailsAsync();
    Task<Lecturer?> GetLecturerByIdWithDetailsAsync(int lecturerId);
    Task<Lecturer?> GetByIdAsync(int id);
}
