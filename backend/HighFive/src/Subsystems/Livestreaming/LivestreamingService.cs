using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Org.OpenAPITools.Models;

namespace src.Subsystems.Livestreaming
{
    public class LivestreamingService: ILivestreamingService
    {
        private readonly IConfiguration _configuration;
        private readonly string _requestBaseUri;
        private string _jwtToken;
        private HttpClient _httpClient;

        public LivestreamingService(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            _requestBaseUri = configuration["LivestreamUri"];
            _jwtToken = GenerateJwt();
            _httpClient = clientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(_requestBaseUri);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_jwtToken);
        }
        
        public async Task<string> AuthenticateUser()
        {
            var requestStr = "{\"email\" :\"";
            requestStr += _configuration["LivestreamAccountDetails:email"] + "\", \"userType\" : \"";
            requestStr += _configuration["LivestreamAccountDetails:userType"] + "\", \"password\" : \"";
            requestStr += _configuration["LivestreamAccountDetails:password"] + "\", \"fullName\" : \"";
            requestStr += _configuration["LivestreamAccountDetails:fullName"] + "\"}";
            var body = new StringContent(requestStr,
                Encoding.UTF8,
                "application/json");
            
            var response = await _httpClient.PostAsync("/rest/v2/users/authenticate", body);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
            return null;
        }

        public async Task CreateApplication(string userId)
        {
            var appName = userId.Replace("-", string.Empty);
            var response = await _httpClient.PostAsync("/rest/v2/applications/" + appName, null);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
            }
        }

        public void UpdateApplicationSettings()
        {
            throw new System.NotImplementedException();
        }

        public string CreateStreamingUrl()
        {
            throw new System.NotImplementedException();
        }

        public string CreateOneTimeToken()
        {
            throw new System.NotImplementedException();
        }

        public string ReturnAllLiveStreams()
        {
            throw new System.NotImplementedException();
        }

        private string GenerateJwt()
        {
            var key = _configuration["LivestreamJWTSecret"];
            const string issuer = "https://high5api.azurewebsites.net";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));    
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var permClaims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(issuer, //Issuer   
                issuer,  //Audience    
                permClaims,    
                expires: DateTime.Now.AddDays(1),    
                signingCredentials: credentials);    
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
        
    }
}