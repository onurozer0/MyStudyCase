namespace BFB.Core.UnitOfWorks
{
	public interface IUnitOfWork
	{
		void Commit();
		Task CommitAsync();
	}
}
