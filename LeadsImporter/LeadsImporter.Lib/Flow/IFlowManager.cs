namespace LeadsImporter.Lib.Flow
{
    public interface IFlowManager
    {
        void Init();
        void ProcessReports();
        void SqlUpdate();
        void Output();
    }
}
