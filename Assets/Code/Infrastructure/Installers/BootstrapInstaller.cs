using Code.Infrastructure.AssetManagement;
using Code.Infrastructure.ConfigsManagement;
using Code.Infrastructure.Identification;
using Code.Infrastructure.Input;
using Code.Infrastructure.Instantiation;
using Code.Infrastructure.SceneManagement;
using Code.Infrastructure.UIManagement;
using Zenject;

namespace Code.Infrastructure.Installers
{
	public class BootstrapInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			BindInfrastructureServices();
			BindInputServices();
			BindUiServices();
		}

		private void BindUiServices()
		{
			Container.BindInterfacesTo<UiService>().AsSingle();
			Container.BindInterfacesTo<WindowInfoService>().AsSingle();
			Container.BindInterfacesTo<UiFactory>().AsSingle();
			Container.BindInterfacesTo<UiRootProvider>().AsSingle();
		}

		private void BindInfrastructureServices()
		{
			Container.BindInterfacesTo<AssetsService>().AsSingle();
			Container.BindInterfacesTo<InstantiateService>().AsSingle();
			Container.BindInterfacesTo<ConfigsService>().AsSingle();
			Container.BindInterfacesTo<SceneLoadService>().AsSingle();
			Container.BindInterfacesTo<IdentifierService>().AsSingle();
		}

		private void BindInputServices()
		{
			Container.BindInterfacesTo<MobileInputService>().AsSingle();
		}
	}
}