namespace Code.Infrastructure.Identification
{
	public class IdentifierService : IIdentifierService
	{
		private int _lastId;

		public int Next() => _lastId += 1;
	}
}