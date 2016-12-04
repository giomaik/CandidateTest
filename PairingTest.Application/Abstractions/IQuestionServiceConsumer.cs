using System.Threading.Tasks;

namespace PairingTest.Application.Abstractions
{
    public interface IQuestionServiceConsumer
    {
        Task<Questionnaire> GetQuestionsAsync();
    }
}