using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LeadsImporter.Lib.Flow
{
    public interface IFlowManager
    {
        void Init();
        void ProcessReports();
        void SqlCheck();
        void Validate();
        void Output();
        void End();
    }
}
