using PairingTest.Application;
using PairingTest.Application.Abstractions;
using PairingTest.Web.Models;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PairingTest.Web.Controllers
{
    public class QuestionnaireController : Controller
    {
        private readonly IQuestionServiceConsumer _questionServiceConsumer;

        public QuestionnaireController(IQuestionServiceConsumer questionServiceConsumer)
        {
            _questionServiceConsumer = questionServiceConsumer;
        }

        public async Task<ViewResult> Index()
        {
            try
            {
                Questionnaire result = (await _questionServiceConsumer.GetQuestionsAsync());
                var viewModel = new QuestionnaireViewModel()
                {
                    QuestionnaireTitle = result.QuestionnaireTitle,
                    QuestionsText = result.QuestionsText
                };

                return View(viewModel);
            }
            catch (HttpException ex)
            {
                return View("Error", ex);
            }
        }
    }
}