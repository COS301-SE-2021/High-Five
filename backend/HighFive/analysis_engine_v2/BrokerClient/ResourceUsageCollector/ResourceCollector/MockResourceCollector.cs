using System;
using analysis_engine.BrokerClient.ResourceUsageCollector.Models;

namespace analysis_engine.BrokerClient.ResourceUsageCollector.ResourceCollector
{
    public class MockResourceCollector
    {
        public PerformanceInfo GetPerformance()
        {
            var random = new Random();
            var isHighUsage = random.Next(1, 4);
            var performanceInfo = new PerformanceInfo();

            int minValue;
            int maxValue;

            if (isHighUsage == 4)
            {
                minValue = 60;
                maxValue = 100;
            }
            else
            {
                minValue = 0;
                maxValue = 60;
            }
            
            performanceInfo.CpuUsage = random.Next(minValue, maxValue);
            performanceInfo.GpuUsage = random.Next(minValue, maxValue);
            performanceInfo.NetUsage = random.Next(minValue, maxValue);
            performanceInfo.DiskUsage = random.Next(minValue, maxValue);

            return performanceInfo;
        }
    }
}