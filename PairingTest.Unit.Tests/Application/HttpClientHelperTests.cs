using Moq;
using NUnit.Framework;
using PairingTest.Application;
using PairingTest.Application.Abstractions;

namespace PairingTest.Unit.Tests.Application
{
    [TestFixture]
    public class HttpClientHelperTests
    {
        [Test]
        public void ShouldUseTheCorrectBaseUrl()
        {
            //Arrange
            string expectedUrl = "http://localhost/";
            var mockedUrlProvider = new Mock<IApiBaseUrlProvider>();
            mockedUrlProvider.Setup(a => a.ApiBaseUrl).Returns(expectedUrl);
            var helper = new HttpClientHelper(mockedUrlProvider.Object);

            //Act
            var requestUri = helper.GetAsync("").Result.RequestMessage.RequestUri;

            //Assert
            Assert.That(requestUri.AbsoluteUri, Is.EqualTo(expectedUrl));
        }
    }
}