using Microsoft.Extensions.Configuration;
using Octokit;

namespace Service
{
    public class GitHubService : IGitHubService
    {
        public readonly GitHubClient _client;
        private readonly IConfiguration _configuration;
        public GitHubService(IConfiguration _configuration)
        {
            _client = new GitHubClient(new ProductHeaderValue("my-github-app"));
            _configuration = _configuration;
        }
        public async Task<int> GetUserFollowersAsync(string userName)
        {
            var user = await _client.User.Get(userName);
            return user.Followers;
        }
        public async Task<int> GetPortfolio(string userName)
        {
            var user = await _client.User.Get(userName);
            return user.PublicRepos ;

        }


        public async Task<List<Repository>> SearchRepositoriesInCSharp()
        {
            var request = new SearchRepositoriesRequest("repo-name") { Language = Language.CSharp };
            var result = await _client.Search.SearchRepo(request);
            return result.Items.ToList();
        }
    }
}
//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
