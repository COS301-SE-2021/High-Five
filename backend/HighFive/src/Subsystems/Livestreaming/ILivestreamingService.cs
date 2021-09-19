using System.Threading.Tasks;

namespace src.Subsystems.Livestreaming
{
    public interface ILivestreamingService
    {
        public Task<string> AuthenticateUser();
        public Task<string> CreateApplication(string userId);
        public Task UpdateApplicationSettings(string appName);

        public Task<string> CreateStreamingUrl(string appName);
        public Task<string> CreateOneTimeToken(string appName, string id, string type);
        public Task<string> ReturnAllLiveStreams(string appName);
    }
}