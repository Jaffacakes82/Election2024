using Election2024.Infrastructure.Interfaces;
using Election2024.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Election2024.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOpenAIService openAIService;

        public HomeController(ILogger<HomeController> logger, IOpenAIService openAIService)
        {
            _logger = logger;
            this.openAIService = openAIService;
        }

        public IActionResult Index()
        {
            return View(new QuestionModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(QuestionModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                var response = await this.openAIService.AskQuestionAsync(model.QuestionText);
                model.QuestionAnswer = response;
            }
            catch (Exception e)
            {
                this._logger.LogCritical(e, "Failed to call OpenAI API service.");
                model.QuestionAnswer = string.Empty;
                model.Failed = true;
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}