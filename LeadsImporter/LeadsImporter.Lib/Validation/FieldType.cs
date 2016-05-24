using System.Diagnostics.CodeAnalysis;

namespace LeadsImporter.Lib.Validation
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum FieldType
    {
        STRING,
        FIXED,
        DATE,
        VALUE
    }
}