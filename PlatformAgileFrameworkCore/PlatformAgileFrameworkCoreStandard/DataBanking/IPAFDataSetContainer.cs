using System.Collections.Generic;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.DataBanking
{
	/// <summary>
	/// Container for data that is really nothing but a dictionary that can be
	/// filled through data queries or through other means.
	/// </summary>
	/// <typeparam name="TDataItem">
	/// The type of data item. Constrained to have access to its key although
	/// not necessarily a key/value type.
	/// </typeparam>
	/// <typeparam name="TPrimaryKey">The type of the primary key.</typeparam>
	/// <threadsafety>Not thread-safe.</threadsafety>
	public interface IPAFDataSetContainer<TDataItem, in TPrimaryKey>
		where TDataItem : class, IPAFProviderPattern<TPrimaryKey>
	{
		IEnumerable<TDataItem> Local { get; }
		string SQLQuery { get; }

		void Add(TDataItem item);
		TDataItem Create();
		void DumpData();
		IEnumerator<TDataItem> ExecuteSqlQuery(string sql, object[] queryParameters);
		TDataItem Find(TPrimaryKey keyValue);
		void Remove(TPrimaryKey key);
	}
}