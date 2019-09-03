using Mikv.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace UnitTests
{
    public class SecretTest
    {
        private readonly Mock<ILogger<SecretController>> logger = new Mock<ILogger<SecretController>>();
        private readonly Mock<IConfiguration> config;
        private readonly SecretController c;

        public SecretTest()
        {
            config = new Mock<IConfiguration>();
            config.SetupGet(x => x[Mikv.Constants.KeyVaultSecretName]).Returns(AssertValues.SecretResult);

            c = new SecretController(logger.Object, config.Object);
        }

        [Fact]
        public void GetSecret()
        {
            var res = (ObjectResult)c.GetSecret();

            OkObjectResult ok = res as OkObjectResult;

            Assert.NotNull(ok);

            string s = ok.Value.ToString();

            Assert.Equal(AssertValues.SecretResult, s);
        }
    }
}
