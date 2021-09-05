
namespace analysis_engine
{
    public interface Tool
    {
        public static Buffer Buffer;
        public Data Process(Data data);
        public void Init();
    }
}