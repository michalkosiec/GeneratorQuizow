using Microsoft.AspNetCore.Mvc;
using System;
using GeneratorQuizow.Models;
using GeneratorQuizow.Services;

namespace GeneratorQuizow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController(IQuizService quizService) : ControllerBase
    {
        [HttpPost("save")]
        public IActionResult SaveQuiz([FromQuery] string? fileName, [FromBody] Quiz quiz)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                    throw new ArgumentException("Nie podano ścieżki do pliku");

                quizService.ValidateAndSaveQuiz(quiz, fileName);
                return Ok(new { Message = $"Quiz '{quiz.Title}' został zaszyfrowany i zapisany jako {fileName}." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"Wystąpił błąd serwera: {ex.Message}" });
            }
        }

        [HttpGet("load")]
        public IActionResult LoadQuiz([FromQuery] string fileName)
        {
            try
            {
                var quiz = quizService.GetQuiz(fileName);
                return Ok(quiz);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = $"Nie udało się wczytać quizu: {ex.Message}" });
            }
        }
    }
}