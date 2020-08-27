using Prism.Ioc;
using Prism.Modularity;
using ModuleTest.Views;
using ModuleTest.ViewModels;

namespace ModuleTest
{
    public class ModuleTestModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ViewA, ViewAViewModel>();
        }
    }
}
