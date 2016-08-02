namespace LeadsImporter.Lib.Flow
{
    public interface IFlowManager
    {
        bool PreCheck();
        void Init();
        void Process();
        void Output();
    }
}
