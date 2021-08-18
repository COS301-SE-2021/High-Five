using System.Threading.Tasks;
using Org.OpenAPITools.Models;

namespace src.Subsystems.Analysis
{
    public interface IAnalysisService
    {
        public Task<string> AnalyzeMedia(AnalyzeMediaRequest request);
        public void SetBaseContainer(string containerName);
    }
}