using System;
using System.Runtime.InteropServices;

namespace DotNetPusher.Pushers
{
    public partial class Pusher
    {
        private static class Interop64
        {
            [DllImport(@"C:\Users\Administrator\RiderProjects\highfive\backend\HighFive\analysis_engine_v2\bin\Debug\NativePusher.x64.dll", EntryPoint = "CreatePusher", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            public static extern int CreatePusher(out IntPtr hPushr);

            [DllImport(@"C:\Users\Administrator\RiderProjects\highfive\backend\HighFive\analysis_engine_v2\bin\Debug\NativePusher.x64.dll", EntryPoint = "DestroyPusher", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            public static extern void DestroyPusher(IntPtr hPushr);

            [DllImport(@"C:\Users\Administrator\RiderProjects\highfive\backend\HighFive\analysis_engine_v2\bin\Debug\NativePusher.x64.dll", EntryPoint = "StartPush", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            public static extern int StartPush(IntPtr hPushr, string url, int width, int height, int frameRate);

            [DllImport(@"C:\Users\Administrator\RiderProjects\highfive\backend\HighFive\analysis_engine_v2\bin\Debug\NativePusher.x64.dll", EntryPoint = "StopPush", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            public static extern int StopPush(IntPtr hPushr);

            [DllImport(@"C:\Users\Administrator\RiderProjects\highfive\backend\HighFive\analysis_engine_v2\bin\Debug\NativePusher.x64.dll", EntryPoint = "PushPacket", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            public static extern int PushPacket(IntPtr hPushr, IntPtr hPacket);
        }
    }
}
