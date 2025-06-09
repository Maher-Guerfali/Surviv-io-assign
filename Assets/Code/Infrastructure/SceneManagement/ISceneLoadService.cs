using System;
using System.Threading.Tasks;

namespace Code.Infrastructure.SceneManagement
{
	public interface ISceneLoadService
	{
		Task LoadScene(string name, Action onLoaded = null);
	}
}