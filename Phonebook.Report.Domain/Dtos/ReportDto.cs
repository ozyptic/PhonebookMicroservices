namespace Phonebook.Report.Domain.Dtos
{
    public class ReportDto
    {

        public string Id { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public IList<ReportDetailDto> ReportDetails { get; set; }
    }
}
