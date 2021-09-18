using System;

namespace iread_assignment_ms.Web.Dto.AttachmentDto
{
    public class AttachmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DownloadUrl { get; set; }
        public string Type { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
        public DateTime UploadDate { get; set; }
    }
}