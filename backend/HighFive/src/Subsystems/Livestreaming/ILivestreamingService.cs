using System.Threading.Tasks;

namespace src.Subsystems.Livestreaming
{
    public interface ILivestreamingService
    {
        public Task<string> AuthenticateUser();
        public Task<string> CreateApplication(string userId);
        public Task UpdateApplicationSettings(string appName);

        public string CreateStreamingUrl();
        public string CreateOneTimeToken();
        public string ReturnAllLiveStreams();
    }
}