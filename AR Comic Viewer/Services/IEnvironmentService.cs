using System.Collections.Generic;

namespace AR_Comic_Viewer.Services
{
    public interface IEnvironmentService
    {
        IEnumerable<string> GetCommandLineArguments();
    }
}
