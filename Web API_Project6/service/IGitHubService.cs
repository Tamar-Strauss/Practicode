using Octokit;

namespace Service
{
    public interface IGitHubService
    {
        Task<int> GetUserFollowersAsync(string userName);
        Task<List<Repository>> SearchRepositoriesInCSharp();
    }
}