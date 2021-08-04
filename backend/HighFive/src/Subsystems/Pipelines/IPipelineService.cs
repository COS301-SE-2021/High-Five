using System.Threading.Tasks;
using Org.OpenAPITools.Models;

namespace src.Subsystems.Pipelines
{
    public interface IPipelineService
    {
        public GetPipelinesResponse GetPipelines();
        public Task<CreatePipelineResponse> CreatePipeline(CreatePipelineRequest request);
        public Task<bool> AddTools(AddToolsRequest request);
        public Task<bool> RemoveTools(RemoveToolsRequest request);
        public Task<bool> DeletePipeline(DeletePipelineRequest request);
        public string[] GetAllTools();
        public void SetBaseContainer(string container);
        public GetPipelineIdsResponse GetPipelineIds();
        public Pipeline GetPipeline(GetPipelineRequest request);
    }
}
