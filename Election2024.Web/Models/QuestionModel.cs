using System.ComponentModel.DataAnnotations;

namespace Election2024.Web.Models;

public class QuestionModel
{
    [Required(ErrorMessage = "Oops, enter a question to ask the politicians!")]
    [MaxLength(100)]
    public string QuestionText { get; set; } = string.Empty;
    public string QuestionAnswer { get; set; } = string.Empty;
}
