using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSecurityWPF.Models
{
    public class TaskItems
    {
        public string Description { get; set; }
        public bool IsCompleted { get; set; }

        // Optional reminder time (nullable)
        public DateTime? ReminderTime { get; set; }

        public string Display =>
            $"{Description} - {(IsCompleted ? "Completed" : "Pending")}" +
            (ReminderTime.HasValue ? $" (Reminder: {ReminderTime.Value:t})" : "");
    }
}