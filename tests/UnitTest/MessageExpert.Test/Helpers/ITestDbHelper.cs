using MessageExpert.Data;

namespace MessageExpert.Test.Helpers
{
	public interface ITestDbHelper
	{
		IDbContextLocator Locator { get; }
		void SaveChanges();
	}
}