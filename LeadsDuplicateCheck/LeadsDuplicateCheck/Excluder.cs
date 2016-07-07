using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace LeadsDuplicateCheck
{
    public class Excluder
    {
        private readonly ILogger _logger;
        private readonly Settings _settings;
        private readonly List<string> _ignoreList;

        public Excluder(ILogger logger, Settings settings)
        {
            _logger = logger;
            _settings = settings;
            _ignoreList = new List<string>();
        }

        public Excluder Read()
        {
            try
            {
                _logger.AddInfo("Excluder >> Read: Reading ignore file...");
                var fileContent = File.ReadAllLines(_settings.IgnoreFilePath);
                _ignoreList.Clear();
                foreach (var s in fileContent)
                {
                    if (!string.IsNullOrWhiteSpace(s)) _ignoreList.Add(s.Trim());
                }
                _logger.AddInfo($"Excluder >> Read: Found {_ignoreList.Count} ignore records.");
            }
            catch (Exception ex)
            {
                _logger.AddError("Excluder >> Read", ex);
            }

            return this;
        }

        public List<Duplicate> Exclude(IEnumerable<Duplicate> duplicates)
        {
            var output = new List<Duplicate>();

            try
            {
                _logger.AddInfo("Excluder >> Exclude: Excluding ignored claims...");
                foreach (var duplicate in duplicates)
                {
                    var caseRef = duplicate.ProclaimClaimData.CaseRef;
                    if(IsIgnored(caseRef)) continue;
                    output.Add(duplicate);
                }

            }
            catch (Exception ex)
            {
                _logger.AddError("Excluder >> Exclude", ex);
            }

            return output;
        }

        private bool IsIgnored(string caseRef)
        {
            return _ignoreList.Contains(caseRef);
        }
    }
}