using System.Collections.Generic;

namespace LeadsImporter.Lib.Validation
{
    public class Validations
    {
        private readonly List<Validation> _validations;

        public Validations()
        {
            _validations = new List<Validation>();
        }

        public void Add(Validation validation)
        {
            _validations.Add(validation);
        }

        public int Count()
        {
            return _validations.Count;
        }
    }
}