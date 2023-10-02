using System;
using Newtonsoft.Json;
using TransactionManager.Models.APIModels.Login.Authentication.External;
using TransactionManager.Models.APIModels.Login.Authorization.TokenResponseModel;

namespace TransactionManager.Service.Login.Authorization
{
	public class ImgurOauth2Service : ExternalOAuthServiceModel<ImgurAPIResponseModel>
	{
        private string grant_type { get; set; }
		public ImgurOauth2Service(string apiEndpoint, string clientId, string clientSecret, string grant_type) : base(apiEndpoint, clientId, clientSecret)
		{
            this.grant_type = grant_type;
		}
        public override ImgurAPIResponseModel? generateAccessToken(string key, string value)
        {
            if (this.api_endpoint == null)
            {
                throw new Exception("API Endpoint invalid");
            }
            HttpClient httpClient = new HttpClient();
            var content = new
            {
                client_id = this.client_id,
                client_secret = this.client_secret,
                grant_type = this.grant_type,
                key = value
            };
            HttpResponseMessage response = httpClient.PostAsJsonAsync(this.api_endpoint, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ImgurAPIResponseModel>(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                throw new Exception($"Failed to exchange code for access token: {response.ReasonPhrase}");
            }
        }
    }
}

