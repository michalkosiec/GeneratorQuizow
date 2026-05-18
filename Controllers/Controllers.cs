using Microsoft.AspNetCore.Mvc;
using System;
using GeneratorQuizow.Models;
using GeneratorQuizow.Services;

namespace GeneratorQuizow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        // POST: api/quiz/save?fileName=moj_quiz.eqz
        [HttpPost("save")]
        public IActionResult SaveQuiz([FromQuery] string fileName, [FromBody] Quiz quiz)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                    fileName = "default_quiz.eqz";

                _quizService.ValidateAndSaveQuiz(quiz, fileName);
                return Ok(new { Message = $"Quiz '{quiz.Title}' został zaszyfrowany i zapisany jako {fileName}." });
            }
            catch (ArgumentException ex)
            {
                // Przyjazna obsługa błędów - ochrona przed pomyłkami (walidacja z warstwy logiki)
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"Wystąpił błąd serwera: {ex.Message}" });
            }
        }

        // GET: api/quiz/load?fileName=moj_quiz.eqz
        [HttpGet("load")]
        public IActionResult LoadQuiz([FromQuery] string fileName)
        {
            try
            {
                var quiz = _quizService.GetQuiz(fileName);
                return Ok(quiz);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = $"Nie udało się wczytać quizu: {ex.Message}" });
            }
        }
    }
}