﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RollingGigApi.ViewModels
{
    public class TodoItemViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public bool IsComplete { get; set; }
    }
}
