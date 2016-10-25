using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Common.Models.Entities
{
    public class Log : BaseEntity
    {
        public string App { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Data { get; set; }
        public string Message { get; set; }
    }
}
