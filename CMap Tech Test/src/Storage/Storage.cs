using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CMap_Tech_Test.src.Storage
{
    public class Storage
    {
        public readonly Dictionary<(Guid userID, DateOnly date), List<Timesheet.Timesheet>> entries;

        public Storage() 
        {
            entries = new Dictionary<(Guid userID, DateOnly date), List<Timesheet.Timesheet>>();
        }

        public void AddEntry(Timesheet.Timesheet entry)
        {
            if (entries.ContainsKey((entry.userID, entry.date)))
            {
                entries[(entry.userID, entry.date)].Add(entry);
            } else
            {
                entries.Add((entry.userID, entry.date), new List<Timesheet.Timesheet> { entry });
            }
        }

        public bool RemoveEntry(Guid userID, DateOnly date)
        {
            return entries.Remove((userID, date));
        }

        public bool EditEntry(Timesheet.Timesheet entry, Guid projectID)
        {
            if (entries.ContainsKey((entry.userID, entry.date)) && entries[(entry.userID, entry.date)].Any(p => p.projectID == projectID))
            {
                var entryIndex = entries[(entry.userID, entry.date)].FindIndex(p => p.projectID == projectID);
                entries[(entry.userID, entry.date)][entryIndex] = entry;
                return true;
            }
            return false;
        }

        public List<Timesheet.Timesheet> ListEntries(Guid userID,  DateOnly weekBeginning)
        {
            List<Timesheet.Timesheet> returnList = new List<Timesheet.Timesheet>();
            for (DateOnly date = weekBeginning; date.DayOfWeek != DayOfWeek.Saturday; date = date.AddDays(1))
            {
                try
                {
                    returnList.AddRange(entries[(userID, date)]);
                }
                catch (KeyNotFoundException)
                {
                    // continue
                }
            }
            return returnList;
        }

        public Decimal TotalHours(Guid userID, DateOnly weekBeginning)
        {
            Decimal totalHours = 0;
            for (DateOnly date = weekBeginning; date.DayOfWeek != DayOfWeek.Saturday; date = date.AddDays(1))
            {
                for (var i = 0; i < entries[(userID, date)].Count; i++)
                {
                    totalHours = totalHours + entries[(userID, date)][i].hoursWorked;
                }
                ;
            }
            return totalHours;
        }

        public static void Main() { }
    }
}
