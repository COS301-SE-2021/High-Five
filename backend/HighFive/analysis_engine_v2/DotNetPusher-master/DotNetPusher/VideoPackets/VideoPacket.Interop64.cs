using System;
using System.Runtime.InteropServices;

namespace DotNetPusher.VideoPackets
{
    public partial class VideoPacket
    {
        private static class Interop64
        {
            [DllImport(@"C:\Users\Administrator\RiderProjects\highfive\backend\HighFive\analysis_engine_v2\bin\Debug\NativePusher.x64.dll", EntryPoint = "GetPacketIndex", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            public static extern int GetPacketIndex(IntPtr hPacket);

            [DllImport(@"C:\Users\Administrator\RiderProjects\highfive\backend\HighFive\analysis_engine_v2\bin\Debug\NativePusher.x64.dll", EntryPoint = "GetPacketSize", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            public static extern int GetPacketSize(IntPtr hPacket);

            [DllImport(@"C:\Users\Administrator\RiderProjects\highfive\backend\HighFive\analysis_engine_v2\bin\Debug\NativePusher.x64.dll", EntryPoint = "GetPacketData", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            public static extern IntPtr GetPacketData(IntPtr hPacket);

            [DllImport(@"C:\Users\Administrator\RiderProjects\highfive\backend\HighFive\analysis_engine_v2\bin\Debug\NativePusher.x64.dll", EntryPoint = "DestroyPacket", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            public static extern int DestroyPacket(IntPtr hPacket);
        }
    }
}
