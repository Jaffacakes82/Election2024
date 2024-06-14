using Election2024.Infrastructure.Interfaces;
using Election2024.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Assistants;
using System.ClientModel;
using System.Text;
using System.Text.RegularExpressions;

namespace Election2024.Infrastructure.Services;

public class OpenAIService : IOpenAIService
{
    private readonly IOptions<OpenAiSettings> openAiSettings;

    public OpenAIService(IOptions<OpenAiSettings> openAiSettings)
    {
        this.openAiSettings = openAiSettings;
    }

    public async Task<string> AskQuestionAsync(string question)
    {
        var openAIClient = new OpenAIClient(this.openAiSettings.Value.ApiToken);
#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        var assistantClient = openAIClient.GetAssistantClient();
#pragma warning restore OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

        var assistant = await assistantClient.GetAssistantAsync(this.openAiSettings.Value.AssistantId);


        var thread = await assistantClient.CreateThreadAndRunAsync(assistant, new ThreadCreationOptions
        {
            InitialMessages = { "Do not include markdown syntax in your response", question }
        });

        do
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));
            thread = assistantClient.GetRun(thread.Value.ThreadId, thread.Value.Id);
        } while (!thread.Value.Status.IsTerminal);

        PageableCollection<ThreadMessage> messages = assistantClient.GetMessages(thread.Value.ThreadId, ListOrder.OldestFirst);
        var answer = string.Empty;
        foreach (ThreadMessage message in messages)
        {
            if (message.Role == MessageRole.Assistant)
            {
                foreach (MessageContent contentItem in message.Content)
                {
                    if (!string.IsNullOrEmpty(contentItem.Text))
                    {
                        answer += contentItem.Text;
                    }
                }
            }
        }

        answer = Regex.Replace(answer, "【.*?】", string.Empty);
        return answer;
    }
}