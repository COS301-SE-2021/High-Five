using Org.OpenAPITools.Models;

namespace src.Subsystems.Pipelines
{
    public interface IPipelineService
    {
        public GetPipelinesResponse GetPipelines();
        public void CreatePipeline(CreatePipelineRequest request);
        public void AddTools(AddToolsRequest request);
        public void RemoveTools(RemoveToolsRequest request);
        public void DeletePipeline(DeletePipelineRequest request);
    }
}