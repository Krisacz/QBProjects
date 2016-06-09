namespace LeadsImporter.Lib.Log
{
    public interface ILogger
    {
        void AddError(string error);
        void AddInfo(string info);
    }
}
