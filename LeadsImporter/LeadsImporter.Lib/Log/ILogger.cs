namespace LeadsImporter.Lib.Log
{
    public interface ILogger
    {
        void AddEmptyLine();
        void AddError(string error);
        void AddInfo(string info);
    }
}
