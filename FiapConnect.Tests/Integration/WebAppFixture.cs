using Microsoft.AspNetCore.Mvc.Testing;

namespace FiapConnect.Tests.Integration
{
    // Fixture compartilhado pelos testes de integração
    public class WebAppFixture : IDisposable
    {
        public WebApplicationFactory<Program> Factory { get; private set; }
        public HttpClient Client { get; private set; }

        public WebAppFixture()
        {
            Factory = new WebApplicationFactory<Program>();
            Client = Factory.CreateClient();
        }

        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
        }
    }
}