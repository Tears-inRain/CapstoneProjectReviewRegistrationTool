using System;
using System.Threading.Tasks;

namespace CapstoneReview.Repository.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ISlotRepository Slots { get; }
    ILecturerRepository Lecturers { get; }
    ITeamRepository Teams { get; }
    Task<int> SaveChangesAsync();
}
