using System;
using System.Runtime.InteropServices;

namespace DotNetPusher.Encoders
{
    public partial class Encoder
    {
        private static class Interop64
        {

            [DllImport(@"C:\Users\Administrator\RiderProjects\highfive\backend\HighFive\analysis_engine_v2\bin\Debug\NativePusher.x64.dll", EntryPoint = "CreateEncoder", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            public static extern int CreateEncoder(out IntPtr hEncoder, int width, int height, int frameRate, int bitRate,FrameEncodedCallback frameEncodedCallback);

            [DllImport(@"C:\Users\Administrator\RiderProjects\highfive\backend\HighFive\analysis_engine_v2\bin\Debug\NativePusher.x64.dll", EntryPoint = "DestroyEncoder", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            public static extern void DestroyEncoder(IntPtr hEncoder);

            [DllImport(@"C:\Users\Administrator\RiderProjects\highfive\backend\HighFive\analysis_engine_v2\bin\Debug\NativePusher.x64.dll", EntryPoint = "AddImage", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            public static extern int AddImage(IntPtr hEncoder, IntPtr imageData, int dataLength);

            [DllImport(@"C:\Users\Administrator\RiderProjects\highfive\backend\HighFive\analysis_engine_v2\bin\Debug\NativePusher.x64.dll", EntryPoint = "FlushEncoder", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            public static extern int FlushEncoder(IntPtr hEncoder);
        }
    }
}
