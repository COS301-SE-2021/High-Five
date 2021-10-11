namespace broker_analysis_client.Client.Models
{
    public class AnalysisToolComposite
    {
        public string ModelPath { get; set; }
        public byte[] ByteData { get; set; }
        public string SourceCode { get; set; }
    }
}