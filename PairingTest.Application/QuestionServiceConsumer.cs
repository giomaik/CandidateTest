using PairingTest.Application.Abstractions;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace PairingTest.Application
{
    public class QuestionServiceConsumer : IQuestionServiceConsumer
    {
        private const string QUESTIONS_PATH = "/api/questions";
        private readonly IHttpClientHelper _clientHelper;

        public QuestionServiceConsumer(IHttpClientHelper clientHelper)
        {
            _clientHelper = clientHelper;
        }

        public async Task<Questionnaire> GetQuestionsAsync()
        {
            HttpResponseMessage response = await _clientHelper.GetAsync(QUESTIONS_PATH);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Questionnaire>();
            }
            else
            {
                throw new HttpException(404, "Could not contact the questionnaire API");
            }
        }
    }
}