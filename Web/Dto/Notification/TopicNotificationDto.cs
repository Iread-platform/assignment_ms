using System.Collections.Generic;

namespace iread_assignment_ms.Web.Dto.Notification
{
    public class TopicNotificationDto
    {
        public string Title { get; set; }

        public string Body
        {
            get; set;
        }
        public int TopicID { get; set; }

        public ExtraDataDto ExtraData { get; set; }

    }
}