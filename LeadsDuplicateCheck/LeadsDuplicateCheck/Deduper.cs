using System;
using System.Collections.Generic;

namespace LeadsDuplicateCheck
{
    public class Deduper
    {
        private readonly ConsoleLogger _logger;

        public Deduper(ConsoleLogger logger)
        {
            _logger = logger;
        }

        public List<Duplicate> GetDuplicates(List<AquariumLeadData> aquarium, List<ProclaimClaimData> proclaim)
        {
            try
            {
                _logger.AddInfo($"Deduper >> GetDuplicates: Comparing {aquarium.Count} Aquarium leads VS {proclaim.Count} Proclaim claims");
                var duplicates = new List<Duplicate>();
                foreach (var a in aquarium)
                {
                    //We can not compare it if we don't have loan date
                    if (a.LoanDate == null) continue;

                    foreach (var p in proclaim)
                    {
                        if (p.RppiLoanDate == null && p.UrscLoanDate == null) continue;
                        if (!IsSameClient(a, p)) continue;
                        if (a.LoanDate != p.RppiLoanDate && a.LoanDate != p.UrscLoanDate) continue;
                        duplicates.Add(new Duplicate(a, p));
                    }
                }
                _logger.AddInfo($"Deduper >> GetDuplicates: Found {duplicates.Count} possible duplicates");
                return duplicates;
            }
            catch (Exception ex)
            {
                _logger.AddError("Deduper >> GetDuplicates", ex);
            }

            return null;
        }

        private bool IsSameClient(AquariumLeadData a, ProclaimClaimData p)
        {
            try
            {
                //Normalize data
                var aSurname = a.Surname.ToLower().Trim();
                var pSurname = p.Surname.ToLower().Trim();
                var aPostcode = a.Postcode.ToLower().Trim();
                var pPostcode = p.Postcode.ToLower().Trim();
                var aDob = a.Dob;
                var pDob = p.Dob;

                //Test 1
                var test1Possible = !string.IsNullOrWhiteSpace(aSurname) && !string.IsNullOrWhiteSpace(pSurname)
                    && !string.IsNullOrWhiteSpace(aPostcode) && !string.IsNullOrWhiteSpace(pPostcode);
                var test1Passed = false;

                if (test1Possible)
                {
                    test1Passed = string.Equals(aSurname, pSurname) && string.Equals(aPostcode, pPostcode);
                }
                if (test1Passed) return true;

                //Test 2
                var test2Possible = !string.IsNullOrWhiteSpace(aSurname) && !string.IsNullOrWhiteSpace(pSurname) && aDob != null && pDob != null;
                var test2Passed = false;

                if (test2Possible)
                {
                    test2Passed = string.Equals(aSurname, pSurname) && aDob == pDob;
                }
                if (test2Passed) return true;

                //Test 3
                var test3Possible = !string.IsNullOrWhiteSpace(aPostcode) && !string.IsNullOrWhiteSpace(pPostcode) && aDob != null && pDob != null;
                var test3Passed = false;

                if (test3Possible)
                {
                    test3Passed = string.Equals(aPostcode, pPostcode) && aDob == pDob;
                }
                if (test3Passed) return true;

                //If none of the above passed - return false - it's not the same client
                return false;
            }
            catch (Exception ex)
            {
                _logger.AddError("Deduper >> IsSameClient", ex);
            }

            return false;
        }
    }
}
