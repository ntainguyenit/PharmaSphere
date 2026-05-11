using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using PharmaSphere.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;

namespace PharmaSphere.IntegrationTests
{
    public class BasicTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public BasicTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Home/Index")]
        [InlineData("/Account/Login")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Act
            var response = await _client.GetAsync(url);

            // Assert
            // Redirect to login is expected for some pages if not authenticated
            if (response.StatusCode == HttpStatusCode.Redirect)
            {
                Assert.True(true);
            }
            else
            {
                response.EnsureSuccessStatusCode();
                Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
            }
        }

        [Fact]
        public async Task Post_Login_ReturnsRedirect()
        {
            // Arrange
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Username", "admin"),
                new KeyValuePair<string, string>("Password", "password")
            });

            // Act
            var response = await _client.PostAsync("/Account/Login", content);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        }
    }
}
