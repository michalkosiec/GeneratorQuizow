namespace GeneratorQuizow.Models
{
    public class Answer
    {
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }

    public class Question
    {
        public string Text { get; set; } = string.Empty;
        public List<Answer> Answers { get; set; } = new List<Answer>();
    }

    public class Quiz
    {
        public string Title { get; set; } = string.Empty;
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}