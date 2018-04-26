using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.FormsTestRunner.Models;

namespace Xamarin.FormsTestRunner.Services
{
	public class MockDataStore : IDataStore<ITestResultSummary>
	{
		List<ITestResultSummary> items;

		public MockDataStore()
		{
			items = new List<ITestResultSummary>();
			var mockItems = new List<ITestResultSummary>
			{
				new TestResultSummary { Id = Guid.NewGuid().ToString(), TestElementName = "First item", Status="This is an item description." },
				new TestResultSummary { Id = Guid.NewGuid().ToString(), TestElementName = "Second item", Status="This is an item description." },
				new TestResultSummary { Id = Guid.NewGuid().ToString(), TestElementName = "Third item", Status="This is an item description." },
				new TestResultSummary { Id = Guid.NewGuid().ToString(), TestElementName = "Fourth item", Status="This is an item description." },
				new TestResultSummary { Id = Guid.NewGuid().ToString(), TestElementName = "Fifth item", Status="This is an item description." },
				new TestResultSummary { Id = Guid.NewGuid().ToString(), TestElementName = "Sixth item", Status="This is an item description." },
			};

			foreach (var item in mockItems)
			{
				items.Add(item);
			}
		}

		public async Task<bool> AddItemAsync(ITestResultSummary item)
		{
			items.Add(item);

			return await Task.FromResult(true);
		}

		public async Task<bool> UpdateItemAsync(ITestResultSummary item)
		{
			var _item = items.Where((ITestResultSummary arg) => arg.Id == item.Id).FirstOrDefault();
			items.Remove(_item);
			items.Add(item);

			return await Task.FromResult(true);
		}

		public async Task<bool> DeleteItemAsync(string id)
		{
			var _item = items.Where((ITestResultSummary arg) => arg.Id == id).FirstOrDefault();
			items.Remove(_item);

			return await Task.FromResult(true);
		}

		public async Task<ITestResultSummary> GetItemAsync(string id)
		{
			return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
		}

		public async Task<IEnumerable<ITestResultSummary>> GetItemsAsync(bool forceRefresh = false)
		{
			return await Task.FromResult(items);
		}
	}
	#region  old mock data store.
	//public class OldMockDataStore : IDataStore<TestResultSummary>
	//{
	//	List<TestResultSummary> items;

	//	public OldMockDataStore()
	//	{
	//		items = new List<TestResultSummary>();
	//		var mockItems = new List<TestResultSummary>
	//		{
	//			new TestResultSummary { Id = Guid.NewGuid().ToString(), TestElementName = "First item", Status="This is an item description." },
	//			new TestResultSummary { Id = Guid.NewGuid().ToString(), TestElementName = "Second item", Status="This is an item description." },
	//			new TestResultSummary { Id = Guid.NewGuid().ToString(), TestElementName = "Third item", Status="This is an item description." },
	//			new TestResultSummary { Id = Guid.NewGuid().ToString(), TestElementName = "Fourth item", Status="This is an item description." },
	//			new TestResultSummary { Id = Guid.NewGuid().ToString(), TestElementName = "Fifth item", Status="This is an item description." },
	//			new TestResultSummary { Id = Guid.NewGuid().ToString(), TestElementName = "Sixth item", Status="This is an item description." },
	//		};

	//		foreach (var item in mockItems)
	//		{
	//			items.Add(item);
	//		}
	//	}

	//	public async Task<bool> AddItemAsync(TestResultSummary item)
	//	{
	//		items.Add(item);

	//		return await Task.FromResult(true);
	//	}

	//	public async Task<bool> UpdateItemAsync(TestResultSummary item)
	//	{
	//		var _item = items.Where((TestResultSummary arg) => arg.Id == item.Id).FirstOrDefault();
	//		items.Remove(_item);
	//		items.Add(item);

	//		return await Task.FromResult(true);
	//	}

	//	public async Task<bool> DeleteItemAsync(string id)
	//	{
	//		var _item = items.Where((TestResultSummary arg) => arg.Id == id).FirstOrDefault();
	//		items.Remove(_item);

	//		return await Task.FromResult(true);
	//	}

	//	public async Task<TestResultSummary> GetItemAsync(string id)
	//	{
	//		return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
	//	}

	//	public async Task<IEnumerable<TestResultSummary>> GetItemsAsync(bool forceRefresh = false)
	//	{
	//		return await Task.FromResult(items);
	//	}
	//}
	#endregion // old mock data store.
}
