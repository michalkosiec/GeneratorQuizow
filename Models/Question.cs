namespace GeneratorQuizow.Models
{
    public class Question
    {
        public string Text { get; set; } = string.Empty;
        public List<Answer> Answers { get; set; } = new List<Answer>();
    }
}