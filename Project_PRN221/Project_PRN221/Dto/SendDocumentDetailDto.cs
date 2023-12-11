namespace Project_PRN221.Dto
{
    public class SendDocumentDetailDto
    {
        public string DocumentNumber { get; set; } = null!;

        public string DocumentTitle { get; set; } = null!;
        public string DocumentDescription { get; set; } = null!;

        public string DocumentType { get; set; } = null!;

        public string DocumentUrl { get; set; } = null!;

        public string UserReceive { get; set; } = null!;
        public string AgenceReceive { get; set; } = null!;
        public string UserSend { get; set; } = null!;

        public DateTime SentDate { get; set; }

        public DateTime IssueDate { get; set; }

        public string HumanSign { get; set; } = null!;

    }
}
