namespace Project_PRN221.Dto
{
	public class SendDocumentDto
	{
		public string DocumentNumber { set; get; } = null!;

		public string CreateDate { set; get; } = null!;
		public string Description { set; get; } = null!;
		public string AgenceReceive { set; get; } = null!;
		public string SendDate { set; get; } = null!;

	}
}
