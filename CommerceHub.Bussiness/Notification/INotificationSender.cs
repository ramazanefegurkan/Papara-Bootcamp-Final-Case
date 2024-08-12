using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.Notification
{
    public interface INotificationSender
    {
        Task SendNotificationAsync(string name,string to, string subject, string body);
    }
}
