using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StravaGPX{
    class Webber{
        private HttpClient http;
        private Uri BaseUri;
        public Webber()
        {
            var handler = new HttpClientHandler();
            handler.DefaultProxyCredentials = CredentialCache.DefaultCredentials;
            http = new HttpClient(handler);
            http.Timeout = TimeSpan.FromSeconds(30d);
        }

        async public Task<string> LoadAsync(string website)
        {
            UriBuilder uriBuilder = new UriBuilder(website);
            if (!GetBaseAddress(website).Equals("")){
                Uri uri = new Uri(GetBaseAddress(website));
                BaseUri = uri;
            }
            Console.WriteLine($"Base address => {BaseUri.ToString()}");
            website = BaseUri.ToString() + GetOtherAddress(website);
            byte[] response = await http.GetByteArrayAsync(website);
            String source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
            source = WebUtility.HtmlDecode(source);
            return source;
        }

        private string GetBaseAddress(string uri)
        {
            UriBuilder uriBuilder = new UriBuilder(uri);
            uriBuilder.Query = String.Empty;
            uriBuilder.Path = String.Empty;
            var baseUri = uriBuilder.Uri;
            return baseUri.ToString();
        }
        private string GetOtherAddress(string uri)
        {
            UriBuilder uriBuilder = new UriBuilder(uri);
            return uriBuilder.Path + uriBuilder.Query;
        }
        
    }
}




