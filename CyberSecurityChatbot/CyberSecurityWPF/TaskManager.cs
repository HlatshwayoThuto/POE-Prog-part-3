using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CyberSecurityWPF.Models;

namespace CyberSecurityWPF
{
    public class TaskManager
    {
        // ObservableCollection allows automatic UI updates in WPF
        public ObservableCollection<TaskItems> Tasks { get; private set; }

        public TaskManager()
        {
            Tasks = new ObservableCollection<TaskItems>();
        }

        // Add a new task
        public void AddTask(string description)
        {
            if (!string.IsNullOrWhiteSpace(description))
            {
                Tasks.Add(new TaskItems { Description = description.Trim(), IsCompleted = false });
            }
        }

        // Mark a task as completed
        public void CompleteTask(TaskItems task)
        {
            if (task != null && Tasks.Contains(task))
            {
                task.IsCompleted = true;
            }
        }

        // Delete a task
        public void DeleteTask(TaskItems task)
        {
            if (task != null && Tasks.Contains(task))
            {
                Tasks.Remove(task);
            }
        }

        // Returns the list of all task descriptions with status
        public List<string> ShowTasks()
        {
            return Tasks.Select(t => t.Display).ToList();
        }
        // Optionally set a reminder on a task
        public void SetReminder(TaskItems task, DateTime reminderTime)
        {
            if (task != null && Tasks.Contains(task))
            {
                task.ReminderTime = reminderTime;
            }
        }
     }
}
