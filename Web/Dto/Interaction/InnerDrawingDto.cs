
using System;

namespace iread_assignment_ms.Web.Dto.Interaction
{
    public class InnerDrawingDto
    {
        public int DrawingId { get; set; }
        public string Comment { get; set; }
        public string AudioId { get; set; }
        public string Points { get; set; }
        public Nullable<double> MaxX { get; set; }
        public Nullable<double> MaxY { get; set; }
        public Nullable<double> MinX { get; set; }
        public Nullable<double> MinY { get; set; }
        public string Color { get; set; }


    }
}