using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadsImporter.Lib.Log;

namespace LeadsImporter.Lib.Executer
{
    public interface IExecuter
    {
        void Start();
        void Stop();
        void Execute();
    }
}
