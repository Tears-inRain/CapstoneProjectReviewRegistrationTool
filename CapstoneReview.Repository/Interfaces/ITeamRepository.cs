using CapstoneReview.Repository.Entities;

namespace CapstoneReview.Repository.Interfaces;

public interface ITeamRepository
{
    Task<List<Team>> GetAllTeamsAsync();
    Task<Team?> GetTeamByIdAsync(int teamId);
    Task<Topic?> GetTopicByTeamIdAsync(int teamId);
}
