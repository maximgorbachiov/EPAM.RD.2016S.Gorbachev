using System;
using System.Diagnostics;

namespace StorageLib.Services
{
    public class LogService
    {
        private static readonly Lazy<LogService> Log = new Lazy<LogService>(() => new LogService());
        private readonly BooleanSwitch boolSwitch;
        private readonly TraceSource traceSource;

        private LogService()
        {
            boolSwitch = new BooleanSwitch("boolSwitch", string.Empty);
            traceSource = new TraceSource("traceSource");
        }

        public static LogService Service => Log.Value;

        public void TraceInfo(string info)
        {
            if (boolSwitch.Enabled)
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, info);
            }
        }
    }
}