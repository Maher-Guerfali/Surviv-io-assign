using Code.Gameplay.Movement.Behaviours;
using Code.Infrastructure.Input;
using UnityEngine;
using Zenject;

namespace Code.Gameplay.Input.Behaviours
{
	public class PlayerInput : MonoBehaviour, IInput, IMovementDirectionProvider
	{
		private IInputService _inputService;

		[Inject]
		private void Construct(IInputService inputService)
		{
			_inputService = inputService;
		}
		
		public Vector2 GetDirection()
		{
			var direction = new Vector2(
				_inputService.GetHorizontalInput(),
				_inputService.GetVerticalInput()
			);
			
			return direction.normalized;
		}

		public void SetDirection(Vector2 direction)
		{
			throw new System.NotImplementedException($"{nameof(PlayerInput)} is not designed to set direction.");
		}
	}
}