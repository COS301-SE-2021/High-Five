using System;
using System.IO;

namespace analysis_engine
{
    public static class ConfigStrings
    {
        public static string ModelDirectory = Environment.CurrentDirectory + @"\..\..\Models";
        
        public const string StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=high5storage;AccountKey=JegWChRoJQREt3BbqwriClYH3CdOvmRlhBE6F2GRlMCKH78mvJuLYWovSZIIXhj/6Zw3+xPKdCgZwRhy6kQXkQ==;EndpointSuffix=core.windows.net";
    }
}