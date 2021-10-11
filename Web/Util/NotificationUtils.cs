
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using iread_assignment_ms.Web.Dto.School;
using System.Text.RegularExpressions;
using System.Linq;

namespace iread_assignment_ms.Web.Util
{
    public static class NotificationUtil
    {



        public static string ClassTopicTitle(ClassDto obj)
        {
            return ProcessTopicName(new string(obj.ClassId + obj.Title));
        }


        public static string ClassTeachersTopicTitle(ClassDto obj)
        {
            return ProcessTopicName(new string(obj.ClassId + "TEACHERS"));
        }

        private static string ProcessTopicName(string name)
        {
            Regex rgx = new Regex(@"[a-zA-Z0-9-_.~%]+");
            var cahrs = name.Where((character) => rgx.IsMatch(character.ToString()));
            string processedName = new string(cahrs.ToArray());
            return processedName;
        }
    }
}