using Microsoft.AspNetCore.Mvc;
using GeneratorQuizow.Models;
using GeneratorQuizow.Services;

namespace GeneratorQuizow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController(IQuizService quizService) : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateQuiz([FromQuery] string? fileName, [FromBody] Quiz quiz)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                    throw new ArgumentException("Nie podano ścieżki do pliku", nameof(fileName));

                if (quizService.QuizExists(fileName))
                    return Conflict(new { Error = "Quiz o tej nazwie już istnieje. Użyj PUT, aby go zaktualizować." });

                quizService.ValidateAndSaveQuiz(quiz, fileName);
                return Created($"/api/Quiz?fileName={fileName}", new { Message = $"Utworzono quiz: {fileName}" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"Wystąpił błąd: {ex.Message}" });
            }
        }

        [HttpPut]
        public IActionResult UpdateQuiz([FromQuery] string? fileName, [FromBody] Quiz quiz)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                    throw new ArgumentException("Nie podano ścieżki do pliku", nameof(fileName));

                if (!quizService.QuizExists(fileName))
                    return NotFound(new { Error = "Quiz nie istnieje. Użyj POST, aby utworzyć nowy." });

                quizService.ValidateAndSaveQuiz(quiz, fileName);
                return Ok(new { Message = $"Zaktualizowano quiz: {fileName}" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"Wystąpił błąd: {ex.Message}" });
            }
        }

        [HttpGet]
        public IActionResult LoadQuiz([FromQuery] string? fileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                    throw new ArgumentException("Nie podano ścieżki do pliku", nameof(fileName));

                var quiz = quizService.GetQuiz(fileName);
                return Ok(quiz);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = $"Nie udało się wczytać quizu: {ex.Message}" });
            }
        }

        [HttpDelete]
        public IActionResult DeleteQuiz([FromQuery] string? fileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                    throw new ArgumentException("Nie podano ścieżki do pliku", nameof(fileName));

                if (!quizService.QuizExists(fileName))
                    return NotFound(new { Error = "Quiz nie istnieje." });

                quizService.DeleteQuiz(fileName);
                return Ok(new { Message = $"Usunięto quiz: {fileName}" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"Wystąpił błąd: {ex.Message}" });
            }
        }
    }
}