using System;
using System.Linq;
using GeneratorQuizow.Models;
using GeneratorQuizow.Repositories;

namespace GeneratorQuizow.Services
{
    public interface IQuizService
    {
        void ValidateAndSaveQuiz(Quiz quiz, string fileName);
        Quiz GetQuiz(string fileName);
        bool QuizExists(string fileName);
        void DeleteQuiz(string fileName);
        byte[] GetEncryptedQuizBytes(string fileName);
    }

    public class QuizService(IQuizRepository repository) : IQuizService
    {
        public void ValidateAndSaveQuiz(Quiz quiz, string fileName)
        {
            if (string.IsNullOrWhiteSpace(quiz.Title))
                throw new ArgumentException("Quiz musi posiadać tytuł.");

            if (quiz.Questions == null || !quiz.Questions.Any())
                throw new ArgumentException("Quiz musi zawierać przynajmniej jedno pytanie.");

            foreach (var question in quiz.Questions)
            {
                if (question.Answers.Count != 4)
                    throw new ArgumentException($"Pytanie '{question.Text}' musi mieć dokładnie 4 odpowiedzi.");

                if (!question.Answers.Any(a => a.IsCorrect))
                    throw new ArgumentException($"Pytanie '{question.Text}' musi mieć przynajmniej jedną poprawną odpowiedź.");
            }

            repository.SaveEncrypted(quiz, fileName);
        }

        public Quiz GetQuiz(string fileName)
        {
            return repository.LoadDecrypted(fileName);
        }

        public bool QuizExists(string fileName)
        {
            return repository.Exists(fileName);
        }

        public void DeleteQuiz(string fileName)
        {
            repository.Delete(fileName);
        }

        public byte[] GetEncryptedQuizBytes(string fileName)
        {
            return repository.GetRawEncryptedBytes(fileName);
        }
    }
}