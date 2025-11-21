
namespace Lab2.Models
{
    public class ReponseApi 
    {
        public bool IsSuccess { get; set; } = true;
        public string Notification { get; set; }
        public object Data { get; set; }
    }
}
