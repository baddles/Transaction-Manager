using System;
namespace TransactionManager.Models.APIModels.Login.Authentication.External
{
	public class ExternalOAuthServiceModel<T>
	{
        protected string api_endpoint { get; set; }
        protected string client_id { get; set; }
        protected string client_secret { get; set; }
        public ExternalOAuthServiceModel(string apiEndpoint, string clientID, string clientSecret)
		{
			this.client_id = clientID;
			this.client_secret = clientSecret;
			this.api_endpoint = apiEndpoint;
		}
		// Should be a virtual function here.
		public virtual T? generateAccessToken(string key, string value)
		{
			throw new NotImplementedException("OAuth Flow Service not implemented");
		}
	}
}

