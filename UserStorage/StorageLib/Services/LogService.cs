using StorageLib.Entities;
using StorageLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace StorageLib.Services
{
    public class LogService : IStorage
    {
        private readonly IMasterStorage masterStorage;
        private readonly BooleanSwitch boolSwitch;
        private readonly TraceSource trace;

        public List<User> Users => masterStorage.Users;

        public LogService(IMasterStorage masterStorage, string boolSwitchName, string traceSource)
        {
            if (masterStorage == null)
            {
                if (boolSwitch.Enabled)
                    trace.TraceEvent(TraceEventType.Error, 0, "User service is null!");
                throw new ArgumentNullException(nameof(masterStorage));
            }
            boolSwitch = new BooleanSwitch("boolSwitch", "");
            trace = new TraceSource("trace");
            this.masterStorage = masterStorage;
        }

        public int AddUser(User user)
        {
            if (boolSwitch.Enabled)
                trace.TraceEvent(TraceEventType.Information, 0, "Add work!");
            return masterStorage.AddUser(user);
        }

        public List<int> SearchBy(IComparer<User> criteria, User user)
        {
            if (boolSwitch.Enabled)
                trace.TraceEvent(TraceEventType.Information, 0, "Search work!");
            return masterStorage.SearchBy(criteria, user);
        }

        public void DeleteUser(int id)
        {
            if (boolSwitch.Enabled)
                trace.TraceEvent(TraceEventType.Information, 0, "Delete work!");
            masterStorage.DeleteUser(id);
        }

        public void Load()
        {
            masterStorage.Load();
        }

        public void Save()
        {
            masterStorage.Save();
        }
    }
}