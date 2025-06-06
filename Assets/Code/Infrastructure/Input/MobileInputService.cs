namespace Code.Infrastructure.Input
{
	public class MobileInputService : IInputService
	{
		private const string HorizontalAxis = "Horizontal";
		private const string VerticalAxis = "Vertical";

		public float GetHorizontalInput() => SimpleInput.GetAxisRaw(HorizontalAxis);
		public float GetVerticalInput() => SimpleInput.GetAxisRaw(VerticalAxis);
	}
}