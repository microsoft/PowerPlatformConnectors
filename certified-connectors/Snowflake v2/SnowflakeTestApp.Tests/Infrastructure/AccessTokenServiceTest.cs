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
            string tenantId = TestData.TenantId;
            string clientId = TestData.ClientId;
            string clientSecret = TestData.ClientSecret;
            string scope = TestData.Scope;

            AccessTokenService service = new AccessTokenService(tenantId, clientId, clientSecret, scope);

            string token = service.GetAccessTokenAsync().GetAwaiter().GetResult();

            Assert.IsTrue(token.StartsWith("ey"));
        }
    }
}
