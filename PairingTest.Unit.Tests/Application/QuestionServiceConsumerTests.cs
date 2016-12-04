using Moq;
using NUnit.Framework;
using PairingTest.Application;
using PairingTest.Application.Abstractions;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace PairingTest.Unit.Tests.Application
{
    [TestFixture]
    public class QuestionServiceConsumerTests
    {
        private readonly JavaScriptSerializer _jss = new JavaScriptSerializer();

        [Test]
        public void ShouldThrow404OnBadResponse()
        {
            //Arrange
            IHttpClientHelper mockClient = GetMockClient(
             new StringContent(""),
             HttpStatusCode.NotFound,
             new MediaTypeWithQualityHeaderValue("application/json"));
            var consumer = new QuestionServiceConsumer(mockClient);

            //Act
            var aggregateException = Assert.Throws<AggregateException>(() => consumer.GetQuestionsAsync().Wait());
            var httpException = aggregateException.InnerExceptions
                                    .FirstOrDefault(x => x.GetType() == typeof(HttpException)) as HttpException;

            //Assert
            Assert.That(httpException, Is.Not.Null);
            Assert.That(httpException.GetHttpCode(), Is.EqualTo((int)HttpStatusCode.NotFound));
        }

        [Test]
        public void ShouldFetchQuestionsFromAPI()
        {
            //Arrange
            var expectedQuestionnaire = new Questionnaire()
            {
                QuestionnaireTitle = "My expected title",
                QuestionsText = new[]
                {
                    "Question 1",
                    "Question 2"
                }
            };
            IHttpClientHelper mockClient = GetMockClient(
                new StringContent(_jss.Serialize(expectedQuestionnaire)),
                HttpStatusCode.OK,
                new MediaTypeWithQualityHeaderValue("application/json"));
            var consumer = new QuestionServiceConsumer(mockClient);

            //Act
            Questionnaire result = consumer.GetQuestionsAsync().Result;

            //Assert
            Assert.That(result.QuestionnaireTitle, Is.EqualTo(expectedQuestionnaire.QuestionnaireTitle));
            Assert.That(result.QuestionsText[0], Is.EqualTo(expectedQuestionnaire.QuestionsText[0]));
            Assert.That(result.QuestionsText[1], Is.EqualTo(expectedQuestionnaire.QuestionsText[1]));
        }

        /// <summary>
        /// Helper function to return a mocked IHttpClient to assist with testing
        /// </summary>
        /// <param name="content">The content the helper should return</param>
        /// <param name="statusCode">The status code it should return</param>
        /// <param name="contentType">The media type for the content</param>
        /// <returns></returns>
        private IHttpClientHelper GetMockClient(
          HttpContent content,
          HttpStatusCode statusCode,
          MediaTypeWithQualityHeaderValue contentType
          )
        {
            var expectedQuestionnaire = new Questionnaire()
            {
                QuestionnaireTitle = "my expected title"
            };
            var expectedMessage = new HttpResponseMessage(statusCode)
            {
                Content = content
            };
            expectedMessage.Content.Headers.ContentType = contentType;
            var mockedClient = new Mock<IHttpClientHelper>();
            mockedClient
                .Setup(a => a.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(expectedMessage));

            return mockedClient.Object;
        }
    }
}