namespace LeadsImporter.Lib.Flow
{
    public interface IFlowManager
    {
        void Init();
        void Process();
        void Output();
    }
}
