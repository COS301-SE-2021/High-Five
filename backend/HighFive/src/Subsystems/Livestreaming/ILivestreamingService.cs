using System.Threading.Tasks;

namespace src.Subsystems.Livestreaming
{
    public interface ILivestreamingService
    {
        public Task<string> AuthenticateUser();
        public Task CreateApplication(string userId);
        public void UpdateApplicationSettings();

        public string CreateStreamingUrl();
        public string CreateOneTimeToken();
        public string ReturnAllLiveStreams();
    }
}