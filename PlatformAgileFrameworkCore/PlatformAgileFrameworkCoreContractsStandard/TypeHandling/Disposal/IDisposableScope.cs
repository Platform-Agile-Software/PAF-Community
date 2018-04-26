using System;

namespace PlatformAgileFramework.TypeHandling.Disposal
{	
	/// <summary>
	/// Gives an opportunity to dispose a resource in a using block with the ability to
	/// change whether the resource is really disposed. It's very handy for locks and
	/// weak references.
	/// </summary>
	public interface IDisposableScope: IDisposable
	{
	    /// <summary>
	    /// This causes the dispose method not to have any effect.
	    /// </summary>
	    void CancelDispose();
	    /// <summary>
	    /// This causes the dispose method to have its original effect.
	    /// </summary>
	    void RenewDispose();
	}
}