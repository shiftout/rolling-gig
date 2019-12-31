using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RollingGigApi.Models
{
    public class TodoItemTag
    {
        public long TodoItemId { get; set; }
        public TodoItem TodoItem { get; set; }

        public long TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
