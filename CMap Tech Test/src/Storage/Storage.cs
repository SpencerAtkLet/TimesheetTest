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
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry), "Entry cannot be null");
            }
            if (entry.date.DayOfWeek == DayOfWeek.Sunday || entry.date.DayOfWeek == DayOfWeek.Saturday)
            {
                //Assuming that weekends are not allowed for timesheet entries (barring overtime)
                throw new ArgumentException("Entry date cannot be a weekend (Saturday or Sunday)", nameof(entry.date));
            }
            if (entries.ContainsKey((entry.userID, entry.date)))
            {
                if (entries[(entry.userID, entry.date)].Any(p => p.projectID == entry.projectID))
                {
                    throw new ArgumentException("An entry for this project already exists for this user and date");
                }
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
            if (weekBeginning.DayOfWeek != DayOfWeek.Monday)
            {
                throw new ArgumentException("weekBeginning must be a Monday");
            }
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
            if (weekBeginning.DayOfWeek != DayOfWeek.Monday)
            {
                throw new ArgumentException("weekBeginning must be a Monday");
            }
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
