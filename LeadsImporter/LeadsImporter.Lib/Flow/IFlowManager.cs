namespace LeadsImporter.Lib.Flow
{
    public interface IFlowManager
    {
        void Init();
        void ProcessReports();
        void SqlCheck();
        void Output();
    }
}
