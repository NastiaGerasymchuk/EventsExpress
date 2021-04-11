﻿using System;
using System.Collections.Generic;
using EventsExpress.Db.Entities;
using EventsExpress.Db.Enums;
using Microsoft.AspNetCore.Http;
using NetTopologySuite.Geometries;

namespace EventsExpress.Core.DTOs
{
    public class EventDto
    {
        public Guid Id { get; set; }

        public bool IsBlocked { get; set; }

        public bool IsReccurent { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public int MaxParticipants { get; set; }

        public int Frequency { get; set; }

        public Periodicity Periodicity { get; set; }

        public IFormFile Photo { get; set; }

        public string PhotoUrl { get; set; }

        public Guid PhotoId { get; set; }

        public Photo PhotoBytes { get; set; }

        public bool IsPublic { get; set; }

        public Point Point { get; set; }

        public LocationType Type { get; set; }

        public Uri OnlineMeeting { get; set; }

        public IEnumerable<CategoryDto> Categories { get; set; }

        public IEnumerable<UserEvent> Visitors { get; set; }

        public IEnumerable<InventoryDto> Inventories { get; set; }

        public IEnumerable<Guid> OwnerIds { get; set; }

        public IEnumerable<User> Owners { get; set; }
    }
}
