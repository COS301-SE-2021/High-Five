using System.IO;
using System.Threading.Tasks;

namespace src.Subsystems.Video
{
    public interface IFFmpegWrapperService
    {
        public Task registerDecoder(Stream input, Stream output);
    }
}