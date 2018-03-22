namespace CMS.Web.Extensions.Components
{
	/// <summary>
	/// Represents a service that can handle client side ajax requests and return a response
	/// that makes sense to the javascript method on the client.
	/// </summary>
	public interface IAjaxService
	{
		/// <summary>Gets the name of the service used to map client calls to the appropriate service.</summary>
		string ServiceName { get; }

		/// <summary>
		/// Defines the output type for the IAjaxService (JSON/Text)
		/// </summary>
		OutputType OutputType { get; }

		/// <summary>Processes the request and returns a response.</summary>
		/// <param name="requestparams">The request to handle, usually the query string collection.</param>
		/// <returns>A response collection that should make sense to the client.</returns>
		object Handle(IRequestParameters requestparams);
	}
}