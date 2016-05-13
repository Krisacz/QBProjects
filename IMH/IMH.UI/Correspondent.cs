using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMH.UI
{
    public class Correspondent
    {
        public string Value { get; set; }
        public string Desc { get; set; }
    }

    public class Correspondents
    {
        private List<Correspondent> _list;

        public Correspondents()
        {
            _list = new List<Correspondent>();
        }

        public void Init(string corrDataPath)
        {
            _list.Clear();
            var lines = File.ReadAllLines(corrDataPath);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var newCorr = new Correspondent();
                newCorr.Value = line.Split(',')[0];
                newCorr.Desc = line.Split(',')[1];
                _list.Add(newCorr);
            }
        }

        public List<string> GetDescList()
        {
            var descList = new List<string>();
            foreach (var item in _list)
            {
                descList.Add(item.Desc);
            }
            return descList;
        }

        public string GetValue(string desc)
        {
            return _list.First(x => x.Desc == desc).Value;
        }
    }

}
