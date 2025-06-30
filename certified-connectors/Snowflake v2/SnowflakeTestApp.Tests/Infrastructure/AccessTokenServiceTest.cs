using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace SnowflakeTestApp.Tests.Infrastructure
{
    [TestClass]
    public class AccessTokenServiceTest
    {

        [TestMethod]
        public void GetToken_ShouldGetAccessToken()
        {
            string tenantId = BaseIntegrationTest.TenantId;
            string clientId = BaseIntegrationTest.ClientId;
            string clientSecret = BaseIntegrationTest.ClientSecret;
            string scope = BaseIntegrationTest.Scope;

            AccessTokenService service = new AccessTokenService(tenantId, clientId, clientSecret, scope);

            string token = service.GetAccessTokenAsync().GetAwaiter().GetResult();

            Assert.IsTrue(token.StartsWith("ey"));
        }
    }
}
