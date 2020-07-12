using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainDatingApp.Helpers
{
    public class MessageParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;

        private int pageSize = 10;

        public int PageSize
        {
            get { return pageSize = 10; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

        //we will filter messages sent based on the userId 
        public int UserId { get; set; }

        public string MessageContainer { get; set; } = "Unread";
    }
}
