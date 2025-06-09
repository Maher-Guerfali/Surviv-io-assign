namespace Code.Infrastructure.UIManagement
{
	public interface IWindowInfoService
	{
		int GetWindowOrder<T>() where T : WindowBase;
		string GetWindowAddress<T>() where T : WindowBase;
	}
}