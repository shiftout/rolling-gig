using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RollingGigApi.ViewModels
{
    public class TodoItemCreateEditViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public bool IsComplete { get; set; }

        public ICollection<TagCreateEditViewModel> Tags { get; set; }

        public TodoItemCreateEditViewModel()
        {
            Tags = new List<TagCreateEditViewModel>();
        }
    }
}
