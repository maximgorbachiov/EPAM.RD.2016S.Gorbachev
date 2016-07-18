using StorageLib.Entities;
using StorageLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Storage
{
    public class LogService : IStorage
    {
        private readonly IStorage userStorage;
        private readonly BooleanSwitch boolSwitch;
        private readonly TraceSource trace;

        public List<User> Users { get { return userStorage.Users; } }

        public LogService(IStorage userStorage)
        {
            if (userStorage == null)
            {
                if (boolSwitch.Enabled)
                    trace.TraceEvent(TraceEventType.Error, 0, "User service is null!");
                throw new ArgumentNullException(nameof(userStorage));
            }
            boolSwitch = new BooleanSwitch("boolSwitch", "");
            trace = new TraceSource("trace");
            this.userStorage = userStorage;
        }

        public int AddUser(User user)
        {
            if (boolSwitch.Enabled)
                trace.TraceEvent(TraceEventType.Information, 0, "Add work!");
            return userStorage.AddUser(user);
        }

        public List<int> SearchBy(IComparer<User> criteria, User user)
        {
            if (boolSwitch.Enabled)
                trace.TraceEvent(TraceEventType.Information, 0, "Search work!");
            return userStorage.SearchBy(criteria, user);
        }

        public void DeleteUser(int id)
        {
            if (boolSwitch.Enabled)
                trace.TraceEvent(TraceEventType.Information, 0, "Delete work!");
            userStorage.DeleteUser(id);
        }
    }
}