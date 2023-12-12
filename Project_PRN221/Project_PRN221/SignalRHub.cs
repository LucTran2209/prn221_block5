using Microsoft.AspNetCore.SignalR;
using Project_PRN221.Models;

namespace Project_PRN221
{
    public class SignalRHub: Hub
    {
        public async Task LoadProfile(User profile)
        {
            // Xử lý sự kiện tại đây
        }

        public async Task LoadReceiveDocs(int userIdReceive, int userIdSend)
        {
            // Xử lý sự kiện tại đây
        }
        public async Task LoadCreateDocs(int Id)
        {
            // Xử lý sự kiện tại đây
        }
    }
}
