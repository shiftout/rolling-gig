using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RollingGigApi.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public bool IsComplete { get; set; }
        public DateTime LastModified { get; set; }
    }
}
