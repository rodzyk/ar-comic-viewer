using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AR_Comic_Viewer.ViewModels
{
    public class AppViewModelLocator
    {
        public AppViewModel AppViewModel
        {
            get { return IocKernel.Get<AppViewModel>(); } // Loading UserControlViewModel will automatically load the binding for IStorage
        }
    }
}
