using System.Collections.Generic;

namespace iread_notifications_ms.Web.DTO
{
    public class SingletNotificationDto
    {
        public string Title { get; set; }

        public string Body
        {
            get; set;
        }
        public int UserId { get; set; }

        public ExtraDataDto ExtraData { get; set; }
    }
}