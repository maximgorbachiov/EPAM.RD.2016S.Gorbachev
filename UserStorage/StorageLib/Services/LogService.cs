using System;
using System.Diagnostics;

namespace StorageLib.Services
{
    public class LogService
    {
        private readonly BooleanSwitch boolSwitch;
        private readonly TraceSource traceSource;

        private static readonly Lazy<LogService> logService = new Lazy<LogService>(() => new LogService());

        public static LogService Service => logService.Value;

        private LogService()
        {
            boolSwitch = new BooleanSwitch("boolSwitch", "");
            traceSource = new TraceSource("traceSource");
        }

        public void TraceInfo(string info)
        {
            if (boolSwitch.Enabled)
                traceSource.TraceEvent(TraceEventType.Information, 0, info);
        }
    }
}