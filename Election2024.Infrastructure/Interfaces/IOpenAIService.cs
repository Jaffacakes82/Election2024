namespace Election2024.Infrastructure.Interfaces;

public interface IOpenAIService
{
    Task<string> AskQuestionAsync(string question);
}
