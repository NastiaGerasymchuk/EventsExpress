﻿using System;
using System.Collections.Generic;
using EventsExpress.Db.Enums;

namespace EventsExpress.Core.DTOs
{
    public class EventFilterViewModel
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public string KeyWord { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public Guid? VisitorId { get; set; }

        public Guid? OwnerId { get; set; }

        public double? X { get; set; }

        public double? Y { get; set; }

        public double? Radius { get; set; }

        public List<string> Categories { get; set; }

        public SortBy SortBy { get; set; }

        public EventStatus Status { get; set; } = EventStatus.Active;
    }
}
