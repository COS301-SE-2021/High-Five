using System.IO;
using System.Threading.Tasks;
using FFMpegCore;
using FFMpegCore.Pipes;

namespace src.Subsystems.Video
{
    public class FFmpegCoreService : IFFmpegWrapperService
    {
        
        public FFmpegCoreService() { }

        /*
        *      Description:
        *  This function registers an input and output stream with the FFmpegCore library.        
        *  Input is read from the input stream and decoded into bitmap frames and piped back through the output stream.
        *      Parameters:
        * -> input : The input stream carrying the video
        * -> output: The output stream carrying the bmp frames   
        */
        public async Task registerDecoder(Stream input, Stream output)
        {
            {
                await FFMpegArguments
                .FromPipeInput(new StreamPipeSource(input))
                .OutputToPipe(new StreamPipeSink(output), options => options
                    .ForceFormat("bmp"))
                .ProcessAsynchronously();
                
            }
        }
        
        
    }
}