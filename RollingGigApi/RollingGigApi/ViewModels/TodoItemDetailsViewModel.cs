using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RollingGigApi.ViewModels
{
    public class TodoItemDetailsViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public bool IsComplete { get; set; }
        public DateTime LastModified { get; set; }

        public ICollection<TagDetailsViewModel> Tags { get; set; }

        public TodoItemDetailsViewModel()
        {
            Tags = new List<TagDetailsViewModel>();
        }
    }
}