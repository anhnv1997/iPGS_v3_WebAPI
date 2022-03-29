using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PGS_WEBAPI.Object
{
    public class GroupDetail
    {
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
        public int OccupiedCount { get; set; }
        public int UnOccupiedCount { get; set; }
    }
}
