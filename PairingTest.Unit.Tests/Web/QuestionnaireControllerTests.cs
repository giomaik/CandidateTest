using Moq;
using NUnit.Framework;
using PairingTest.Application;
using PairingTest.Application.Abstractions;
using PairingTest.Web.Controllers;
using PairingTest.Web.Models;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace PairingTest.Unit.Tests.Web
{
    [TestFixture]
    public class QuestionnaireControllerTests
    {
        [Test]
        public void ShouldPutQuestionsInViewData()
        {
            //Arrange
            var expectedQuestionnaire = new Questionnaire()
            {
                QuestionnaireTitle = "My expected questions",
                QuestionsText = new[]
                {
                    "Question 1",
                    "Question 2"
                }
            };
            Mock<IQuestionServiceConsumer> mockedConsumer = new Mock<IQuestionServiceConsumer>();
            mockedConsumer
                .Setup(a => a.GetQuestionsAsync())
                .Returns(Task.FromResult(expectedQuestionnaire));

            var questionnaireController = new QuestionnaireController(mockedConsumer.Object);

            //Act
            var result = (QuestionnaireViewModel)questionnaireController.Index().Result.ViewData.Model;

            //Assert
            Assert.That(result.QuestionnaireTitle, Is.EqualTo(expectedQuestionnaire.QuestionnaireTitle));
            Assert.That(result.QuestionsText[0], Is.EqualTo(expectedQuestionnaire.QuestionsText[0]));
            Assert.That(result.QuestionsText[1], Is.EqualTo(expectedQuestionnaire.QuestionsText[1]));
        }

        [Test]
        public void ShouldReturnErrorPageOnConsumerError()
        {
            //Arrange
            var expectedException = new HttpException((int)HttpStatusCode.NotFound, "Not found");
            Mock<IQuestionServiceConsumer> mockedConsumer = new Mock<IQuestionServiceConsumer>();
            mockedConsumer
                .Setup(a => a.GetQuestionsAsync())
                .Throws(expectedException);
            var questionnaireController = new QuestionnaireController(mockedConsumer.Object);

            //Act
            var result = questionnaireController.Index().Result;

            //Assert
            Assert.That(result.ViewName, Is.EqualTo("Error"));
            Assert.That(result.Model, Is.EqualTo(expectedException));
        }
    }
}