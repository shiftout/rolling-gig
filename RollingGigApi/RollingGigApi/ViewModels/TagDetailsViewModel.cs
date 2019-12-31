using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RollingGigApi.ViewModels
{
    public class TagDetailsViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime LastModified { get; set; }
    }
}
