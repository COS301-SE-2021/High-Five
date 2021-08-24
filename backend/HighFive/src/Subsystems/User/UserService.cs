using System;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Org.OpenAPITools.Models;
using src.Storage;

namespace src.Subsystems.User
{
    public class UserService: IUserService
    {
        private readonly IStorageManager _storageManager;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;
        
        public UserService(IStorageManager storageManager, IConfiguration configuration)
        {
            _storageManager = storageManager;
            _configuration = configuration;
            _client = new HttpClient();
        }

        public GetAllUsersResponse GetAllUsers()
        {
            var tenantId = _configuration.GetValue<string>("AzureAdB2C:TenantId");
            var clientId = _configuration.GetValue<string>("AzureAdB2C:ClientId");
            var clientSecret = _configuration.GetValue<string>("AzureAdB2C:ClientSecret");
            
            //get access token from Azure AD 
            var reqContent = @"grant_type=client_credentials&resource=https://graph.microsoft.com&client_id="+ clientId + "&client_secret="+ System.Web.HttpUtility.UrlEncode(clientSecret);
            var Content = new StringContent(reqContent, Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = _client.PostAsync("https://login.microsoftonline.com/"+ tenantId + "/oauth2/token", Content).Result;
            var tokenString = response.Content.ReadAsStringAsync().Result;
            var token = JsonConvert.DeserializeObject<TokenResult>(tokenString);
           
            //Use access token to call microsoft graph api 
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.access_token);
            Console.WriteLine(_client.GetAsync("https://graph.microsoft.com/v1.0/users").Result.Content.ReadAsStringAsync().Result);

            return null;
        }

        public void DeleteMedia(UserRequest request)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(UserRequest request)
        {
            throw new NotImplementedException();
        }

        public void UpgradeToAdmin(UserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}