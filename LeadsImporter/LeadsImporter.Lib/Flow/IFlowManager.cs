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
        void GetReportsData();
        void SqlCheck();
        void Output();
        void End();
    }
}
