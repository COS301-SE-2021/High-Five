using System.IO;
using System.Threading.Tasks;
using FFMpegCore;
using FFMpegCore.Pipes;

namespace src.Subsystems.Video
{
    public class FFmpegCoreService : IFFmpegWrapperService
    {
        
        public FFmpegCoreService() { }

        public async Task registerDecoder(Stream input, Stream output)
        {
            {
                await FFMpegArguments
                .FromPipeInput(new StreamPipeSource(input))
                .OutputToPipe(new StreamPipeSink(output), options => options
                    .WithVideoCodec("bmp")
                    .ForceFormat("webm"))
                .ProcessAsynchronously();
                
            }
        }
        
        
    }
}