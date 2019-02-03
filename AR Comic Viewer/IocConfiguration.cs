using AR_Comic_Viewer.Services;
using Ninject;
using Ninject.Modules;

namespace AR_Comic_Viewer
{
    class IocConfiguration : NinjectModule
    {
        public override void Load()
        {
            Bind<IDialogService>().To<DefaultDialogService>().InSingletonScope(); 
            Bind<IImageArchiveService>().To<ImageArchiveService>().InTransientScope(); 
            Bind<IEnvironmentService>().To<AppEnvironmentService>().InTransientScope();
        }
    }
}
