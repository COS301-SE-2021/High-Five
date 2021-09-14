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
        public bool SetBaseContainer(string containerName);
        public GetPipelineIdsResponse GetPipelineIds();
        public Task<Pipeline> GetPipeline(GetPipelineRequest request);
        public void StoreUserInfo(string id, string displayName, string email);
        public Task<bool> SetLivePipeline(GetPipelineRequest request);
    }
}
