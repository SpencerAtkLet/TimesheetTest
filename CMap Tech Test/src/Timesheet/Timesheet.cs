using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMap_Tech_Test.src.Timesheet
{
    public class Timesheet
    {
        public Guid userID;
        public Guid projectID;
        public DateOnly date;
        public Decimal hoursWorked;
        public String? description;

        public Timesheet(Guid UserID, Guid ProjectID, DateOnly Date, Decimal HoursWorked, String? Description)
        {
            this.userID = UserID;
            this.projectID = ProjectID;
            this.date = Date;
            this.hoursWorked = HoursWorked;
            this.description = Description;
        }
    }
}
