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
using Newtonsoft.Json.Linq;
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

        public async Task<string> CreateApplication(string userId)
        {
            var appName = userId.Replace("-", string.Empty);
            var response = await _httpClient.PostAsync("/rest/v2/applications/" + appName, null);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
            }
            return appName;
        }

        public async Task UpdateApplicationSettings(string appName)
        {
            var body = new StringContent(DefaultApplicationSettings,
                Encoding.UTF8,
                "application/json");
            var response = await _httpClient.PostAsync("/rest/v2/applications/settings/" + appName, body);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<string> CreateStreamingUrl(string appName)
        {
            var body = new StringContent(CreateStreamingUrlParameters,
                Encoding.UTF8,
                "application/json");
            var requestUri =  appName + "/rest/v2/broadcasts/create?autoStart=false";
            var response = await _httpClient.PostAsync(requestUri, body);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var responseObj = JObject.Parse(responseString);
                return (string)responseObj.GetValue("streamId");
            }
            return "";
        }

        public async Task<string> CreateOneTimeToken(string appName, string id, string type)
        {
            var expireDate = "1633597507";//DateTime.Now.AddHours(1);
            var requestUri = appName + "/rest/v2/broadcasts/" + id + "/token?id=" + id + "&expireDate=" + expireDate + "&type=" + type;
            var response = await _httpClient.GetAsync(requestUri);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var responseObj = JObject.Parse(responseString);
                return (string)responseObj.GetValue("tokenId");
            }
            return "";
        }

        public async Task<string> ReturnAllLiveStreams(string appName)
        {
            var requestUri = "/rest/v2/applications/live-streams/" + appName;
            var response = await _httpClient.GetAsync(requestUri);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
            return null;
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
        
        private const string DefaultApplicationSettings = @"{
            ""remoteAllowedCIDR"": ""127.0.0.1,0.0.0.0/0"",
            ""mp4MuxingEnabled"": false,
            ""webMMuxingEnabled"": false,
            ""addDateTimeToMp4FileName"": false,
            ""hlsMuxingEnabled"": true,
            ""encoderSettingsString"": ""[]"",
            ""hlsListSize"": ""5"",
            ""hlsTime"": ""2"",
            ""dashSegDuration"": ""6"",
            ""dashFragmentDuration"": ""0.5"",
            ""targetLatency"": ""3.5"",
            ""dashWindowSize"": ""5"",
            ""dashExtraWindowSize"": ""5"",
            ""lLDashEnabled"": true,
            ""lLHLSEnabled"": false,
            ""hlsEnabledViaDash"": false,
            ""useTimelineDashMuxing"": false,
            ""webRTCEnabled"": true,
            ""useOriginalWebRTCEnabled"": false,
            ""deleteHLSFilesOnEnded"": true,
            ""deleteDASHFilesOnEnded"": true,
            ""tokenHashSecret"": """",
            ""hashControlPublishEnabled"": false,
            ""hashControlPlayEnabled"": false,
            ""listenerHookURL"": """",
            ""acceptOnlyStreamsInDataStore"": false,
            ""acceptOnlyRoomsInDataStore"": false,
            ""publishTokenControlEnabled"": true,
            ""playTokenControlEnabled"": false,
            ""timeTokenSubscriberOnly"": false,
            ""enableTimeTokenForPlay"": false,
            ""enableTimeTokenForPublish"": false,
            ""timeTokenPeriod"": 60,
            ""hlsPlayListType"": """",
            ""facebookClientId"": ""1898164600457124"",
            ""facebookClientSecret"": ""6c02b406d3e94426c5553c3c9bc17345"",
            ""periscopeClientId"": ""Q90cMeG2gUzC6fImXcp2SvyVqwVSvGDlsFsRF4Uia9NR1M-Zru"",
            ""periscopeClientSecret"": ""dBCjxFbawo436VSWMvuD5SDSZoSdhew_-Fvrh5QhrBXuKoelVM"",
            ""youtubeClientId"": ""183604002006-3ojdgvmqp7rcc6d66atkkhk7p0btie9j.apps.googleusercontent.com"",
            ""youtubeClientSecret"": ""HDwoClZhJzPshtmnWjSJSHjx"",
            ""vodFolder"": """",
            ""previewOverwrite"": false,
            ""stalkerDBServer"": """",
            ""stalkerDBUsername"": """",
            ""stalkerDBPassword"": """",
            ""objectDetectionEnabled"": false,
            ""createPreviewPeriod"": 5000,
            ""restartStreamFetcherPeriod"": 0,
            ""startStreamFetcherAutomatically"": true,
            ""streamFetcherBufferTime"": 0,
            ""mySqlClientPath"": ""/usr/local/antmedia/mysql"",
            ""muxerFinishScript"": """",
            ""webRTCFrameRate"": 30,
            ""webRTCPortRangeMin"": 0,
            ""webRTCPortRangeMax"": 0,
            ""stunServerURI"": ""stun:stun1.l.google.com:19302"",
            ""webRTCTcpCandidatesEnabled"": false,
            ""webRTCSdpSemantics"": ""planB"",
            ""portAllocatorFlags"": 0,
            ""collectSocialMediaActivity"": false,
            ""encoderName"": null,
            ""encoderPreset"": null,
            ""encoderProfile"": null,
            ""encoderLevel"": null,
            ""encoderRc"": null,
            ""encoderSpecific"": null,
            ""encoderThreadCount"": 0,
            ""encoderThreadType"": 0,
            ""vp8EncoderSpeed"": 4,
            ""vp8EncoderDeadline"": ""realtime"",
            ""vp8EncoderThreadCount"": 1,
            ""previewHeight"": 480,
            ""generatePreview"": false,
            ""writeStatsToDatastore"": true,
            ""encoderSelectionPreference"": ""'gpu_and_cpu'"",
            ""allowedPublisherCIDR"": """",
            ""excessiveBandwidthValue"": 300000,
            ""excessiveBandwidthCallThreshold"": 3,
            ""excessiveBandwithTryCountBeforeSwitchback"": 4,
            ""excessiveBandwidthAlgorithmEnabled"": false,
            ""packetLossDiffThresholdForSwitchback"": 10,
            ""rttMeasurementDiffThresholdForSwitchback"": 20,
            ""replaceCandidateAddrWithServerAddr"": false,
            ""appName"": ""test5"",
            ""encodingTimeout"": 5000,
            ""webRTCClientStartTimeoutMs"": 5000,
            ""defaultDecodersEnabled"": false,
            ""updateTime"": 1630957322702,
            ""encoderSettings"": [],
            ""httpForwardingExtension"": ""''"",
            ""httpForwardingBaseURL"": ""''"",
            ""maxAnalyzeDurationMS"": 1500,
            ""disableIPv6Candidates"": true,
            ""rtspPullTransportType"": ""tcp"",
            ""maxResolutionAccept"": 0,
            ""h264Enabled"": true,
            ""vp8Enabled"": false,
            ""h265Enabled"": false,
            ""dataChannelEnabled"": false,
            ""dataChannelPlayerDistribution"": ""all"",
            ""rtmpIngestBufferTimeMs"": 0,
            ""h265EncoderPreset"": null,
            ""h265EncoderProfile"": null,
            ""h265EncoderRc"": null,
            ""h265EncoderSpecific"": null,
            ""h265EncoderLevel"": null,
            ""heightRtmpForwarding"": 360,
            ""audioBitrateSFU"": 96000,
            ""dashMuxingEnabled"": true,
            ""aacEncodingEnabled"": true,
            ""gopSize"": 0,
            ""constantRateFactor"": ""23"",
            ""webRTCViewerLimit"": -1,
            ""toBeDeleted"": false,
            ""jwtSecretKey"": ""J2oSWIqYUijXtvvtSZy637hQBzWMQXlz"",
            ""jwtControlEnabled"": true,
            ""ipFilterEnabled"": false,
            ""ingestingStreamLimit"": -1,
            ""webRTCKeyframeTime"": 2000,
            ""jwtStreamSecretKey"": ""C8UComX0XA5Xbi4mHOI0oNoDSvXm29Ph"",
            ""publishJwtControlEnabled"": false,
            ""playJwtControlEnabled"": true,
            ""dashHttpStreaming"": true,
            ""s3StreamsFolderPath"": ""streams"",
            ""s3PreviewsFolderPath"": ""previews"",
            ""dashHttpEndpoint"": null,
            ""forceDecoding"": false,
            ""s3RecordingEnabled"": false,
            ""s3AccessKey"": """",
            ""s3SecretKey"": """",
            ""s3BucketName"": """",
            ""s3RegionName"": """",
            ""s3Endpoint"": """",
            ""hlsEncryptionKeyInfoFile"": """",
            ""jwksURL"": null,
            ""forceAspectRatioInTranscoding"": false,
            ""webhookAuthenticateURL"": """",
            ""hlsFlags"": ""delete_segments"",
            ""dataChannelWebHook"": null
           }";

        private const string CreateStreamingUrlParameters = @"{
            ""type"": ""liveStream"",
            ""publishType"": ""WebRTC"",
            ""name"": ""S0"",
            ""description"": ""Livestream"",
            ""publish"": true
           }";
    }
}