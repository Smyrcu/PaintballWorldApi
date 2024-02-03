using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
namespace PaintballWorld.Infrastructure.Clients
{
    public class GmailApiClient
    {
        private readonly string _jsonKeyFilePath;
        private readonly string _tokenFilePath;
        private readonly string _applicationName;
        private GmailService _service;

        public GmailApiClient(string jsonKeyFilePath, string tokenFilePath, string applicationName)
        {
            _jsonKeyFilePath = jsonKeyFilePath;
            _tokenFilePath = tokenFilePath;
            _applicationName = applicationName;
            _service = Initialize().Result;
        }

        private async Task<GmailService> Initialize()
        {
            UserCredential credential;
            using (var stream = new FileStream(_jsonKeyFilePath, FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    new[] { GmailService.Scope.GmailModify },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(_tokenFilePath, true)
                );
            }

            return new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = _applicationName,
            });
        }

        public GmailService GetService()
        {
            return _service;
        }
    }
}
