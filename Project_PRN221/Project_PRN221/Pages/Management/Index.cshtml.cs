using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project_PRN221.Models;

namespace Project_PRN221.Pages.Management
{
    public class IndexModel : PageModel
    {
        private readonly Project_PRN221.Models.PROJECT_SENT_DOCUMENTContext _context;

        public IndexModel(Project_PRN221.Models.PROJECT_SENT_DOCUMENTContext context)
        {
            _context = context;
        }


        public int DocNumber { get; set; }

        public int AgenceNumber { get; set; }

        public int SendNumber { get; set; }
		public DateTime weekTime { get; set; }

		public List<int> weekSend { get; set; } = default!;


		public async Task OnGetAsync(string? weekTime)
		{
			DocNumber = _context.Documents.Count();
			AgenceNumber = _context.Agences.Count();
			if (String.IsNullOrEmpty(weekTime))
			{
				this.weekTime = DateTime.Now;
			}
			else
			{
				this.weekTime = ParseWeekTime(weekTime);
			}
			// Xác định ngày đầu tiên và cuối cùng của tuần
			DateTime startOfWeek = this.weekTime.AddDays(-(int)this.weekTime.DayOfWeek);
			DateTime endOfWeek = startOfWeek.AddDays(7);
			weekSend = new List<int>();
			// Lấy số lượng SendDocuments từng ngày trong tuần
			for (DateTime date = startOfWeek; date < endOfWeek; date = date.AddDays(1))
			{
				int count = _context.SendDocuments.Count(d => d.SentDate.Date == date.Date);
				weekSend.Add(count);
				Console.WriteLine($"Ngày {date:yyyy-MM-dd}: {count} SendDocuments");
			}
			SendNumber = _context.SendDocuments.Count();
		}
		public static DateTime ParseWeekTime(string weekTimeString)
		{
			// Kiểm tra xem chuỗi có đúng định dạng không
			if (!Regex.IsMatch(weekTimeString, @"^\d{4}-W\d{2}$"))
			{
				return DateTime.Now;
			}

			// Tách chuỗi thành năm và tuần
			string[] parts = weekTimeString.Split('-');
			int year = int.Parse(parts[0]);
			int week = int.Parse(parts[1].Substring(1));

			// Tính ngày đầu tiên của năm
			DateTime startOfYear = new DateTime(year, 1, 1);

			// Tính ngày đầu tiên của tuần
			int daysOffset = DayOfWeek.Monday - startOfYear.DayOfWeek;

			DateTime firstMonday = startOfYear.AddDays(daysOffset);
			var cal = CultureInfo.CurrentCulture.Calendar;
			int firstWeek = cal.GetWeekOfYear(firstMonday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

			var weekNum = week;
			if (firstWeek <= 1)
			{
				weekNum -= 1;
			}
			var result = firstMonday.AddDays(weekNum * 7);

			// Kiểm tra xem tuần có chứa ngày từ năm tiếp theo không
			if (result.Year > year)
			{
				result = result.AddDays(-7);
			}

			return result;
		}


	}


}
