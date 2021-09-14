using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace iread_assignment_ms.Web.DTO.Story
{
    public class ViewStoryDto
    {
        public int StoryId { get; set; }

        public string Title { get; set; }

    }
}