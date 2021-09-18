using System;
using System.Collections.Generic;
using iread_assignment_ms.Web.Dto.TagDto;

namespace iread_assignment_ms.Web.Dto.StoryDto
{
    public class FullStoryDto
    {
        public int StoryId { get; set; }

        public string Title { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Description { get; set; }

        public int StoryLevel { get; set; }

        public string Writer { get; set; }

        public List<TagWithIdDto> KeyWords { get; set; }

        public AttachmentDto.AttachmentDto StoryCover { get; set; }
        public AttachmentDto.AttachmentDto StoryAudio { get; set; }
        public StoryReview Rating { get; set; }
    }
}