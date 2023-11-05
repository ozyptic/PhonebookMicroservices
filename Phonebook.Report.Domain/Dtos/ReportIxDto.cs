namespace Phonebook.Report.Domain.Dtos
{
    public class ReportIxDto
    {
        public string? Id { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        
    }
}
