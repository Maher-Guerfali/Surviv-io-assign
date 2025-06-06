using UnityEngine;

namespace Code.Gameplay.Cameras.Services
{
	public interface ICameraProvider
	{
		Camera MainCamera { get; }
		float WorldScreenHeight { get; }
		float WorldScreenWidth { get; }
		void SetMainCamera(Camera camera);
	}
}