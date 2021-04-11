﻿using System;
using EventsExpress.ViewModels.Base;

namespace EventsExpress.ViewModels
{
    public class EventPreviewViewModel : EventViewModelBase
    {
        public Guid Id { get; set; }

        public string PhotoUrl { get; set; }

        public bool IsBlocked { get; set; }

        public int CountVisitor { get; set; }
    }
}
