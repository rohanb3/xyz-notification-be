using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xyzies.Notification.Services.Models;

namespace Xyzies.Notification.Services.Helpers
{
    public static class MailerParcer
    {
        public static string ProcessTemplate(string template, Dictionary<string, string> data)
        {
            return Regex.Replace(template, @"\{(.+?)\}", m =>
               m.Groups.Count > 1 && data.ContainsKey(m.Groups[1].Value) ?
               data[m.Groups[1].Value] : m.Value);
        }

        public static Dictionary<string, string> PrepareDictionaryParams(EmailParameters emailParams)
        {
            return typeof(EmailParameters)
                .GetProperties()
                .ToDictionary(c => c.Name.ToLower(), c => c.GetValue(emailParams).ToString());
            
        }
    }
}
