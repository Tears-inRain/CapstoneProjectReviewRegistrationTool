using System.Collections.Generic;
using System.Threading.Tasks;
using CapstoneReview.Service.DTOs;

namespace CapstoneReview.Service.Interfaces;

public interface IMasterDataService
{
    Task<List<MasterLecturerDto>> GetAllLecturersAsync();
    Task<List<MasterTeamDto>> GetAllTeamsAsync();
}
