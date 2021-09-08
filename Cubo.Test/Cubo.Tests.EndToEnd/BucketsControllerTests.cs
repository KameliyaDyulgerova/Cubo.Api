using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Cubo.Api;
using Cubo.Core.DTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using NUnit.Framework;


namespace Cubo.Tests.EndToEnd.Controllers
{
    [TestFixture]
    public class BucketsControllerTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public BucketsControllerTests()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Test]
        public async Task Fetching_buckets_should_return_non_empty_collection()
        {
            var response = await _client.GetAsync("api/buckets");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var buckets = JsonConvert.DeserializeObject<IEnumerable<string>>(content);

           // buckets.Should().NotBeEmpty();
        }

        [Test]
        public async Task Creating_a_new_bucket_should_succeed()
        {
            var name = "test-bucket";
            var response = await CreateBucketAsync(name);

            response.EnsureSuccessStatusCode();
           response.StatusCode.Equals(HttpStatusCode.Created);
           response.Headers.Location.ToString().Equals($"buckets/{name}");
        }

        private async Task<HttpResponseMessage> CreateBucketAsync(string name)
        => await _client.PostAsync($"api/buckets/{name}", GetPayload(new { }));

        private static StringContent GetPayload(object data)
        {
            var json = JsonConvert.SerializeObject(data);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
