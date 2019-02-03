using System;
using System.Collections.Generic;

namespace AR_Comic_Viewer.Services
{
    class AppEnvironmentService : IEnvironmentService
    {
        public IEnumerable<string> GetCommandLineArguments()
        {
            return Environment.GetCommandLineArgs();
        }
    }
}
