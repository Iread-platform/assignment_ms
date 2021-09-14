using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace iread_assignment_ms.Web.Dto.AttachmentDto
{
    public class AttachmentsWithStoryId
    {
        public AttachmentsWithStoryId(int storyId, List<IFormFile> attachment)
        {
            this.Attachment = attachment;
            this.StoryId = storyId;
        }

        public int StoryId;
        public List<IFormFile> Attachment;
    }
}