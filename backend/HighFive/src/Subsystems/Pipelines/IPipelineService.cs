using System.Threading.Tasks;
using Org.OpenAPITools.Models;

namespace src.Subsystems.Pipelines
{
    public interface IPipelineService
    {
        public GetPipelinesResponse GetPipelines();
        public void CreatePipeline(CreatePipelineRequest request);
        public bool AddTools(AddToolsRequest request);
        public bool RemoveTools(RemoveToolsRequest request);
        public Task<bool> DeletePipeline(DeletePipelineRequest request);
    }
}