using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProclaimAuditConverter
{
    public class Case
    {
        public string CaseRef { get; private set; }
        public string FieldName { get; private set; }
        public List<Change> Changes { get; private set; }

        public Case(string caseRef, string fieldName)
        {
            CaseRef = caseRef;
            FieldName = fieldName;
            Changes = new List<Change>();
        }

        public void AddChange(DateTime date, TimeSpan time, string by, string source, string newValue)
        {
            var change = new Change(date, time, by, source, newValue);
            Changes.Add(change);
        }

        public Change GetLatest()
        {
            Change latestChange = null;

            foreach (var change in Changes)
            {
                if (latestChange == null)
                {
                    latestChange = change;
                }
                else
                {
                    if (latestChange.Date > change.Date)
                    {
                        //Do nothing - latestChange stays the same
                    }
                    else if (latestChange.Date == change.Date)
                    {
                        //Check time
                        if (latestChange.Time > change.Time)
                        {
                            //Do nothing - latestChange stays the same
                        }
                        else if (latestChange.Time == change.Time)
                        {
                            //Do nothing - latestChange stays the same
                        }
                        else if (latestChange.Time < change.Time)
                        {
                            latestChange = change;
                        }
                    }
                    else if (latestChange.Date < change.Date)
                    {
                        latestChange = change;
                    }
                }
            }

            //Return latest change
            return latestChange;
        }

        public Change GetOldest()
        {
            Change oldestChange = null;

            foreach (var change in Changes)
            {
                if (oldestChange == null)
                {
                    oldestChange = change;
                }
                else
                {
                    if (oldestChange.Date > change.Date)
                    {
                        oldestChange = change;
                    }
                    else if (oldestChange.Date == change.Date)
                    {
                        //Check time
                        if (oldestChange.Time > change.Time)
                        {
                            oldestChange = change;
                        }
                        else if (oldestChange.Time == change.Time)
                        {
                            //Do nothing - latestChange stays the same
                        }
                        else if (oldestChange.Time < change.Time)
                        {
                            //Do nothing - latestChange stays the same
                        }
                    }
                    else if (oldestChange.Date < change.Date)
                    {
                        //Do nothing - latestChange stays the same
                    }
                }
            }

            //Return latest change
            return oldestChange;
        }
    }
}
