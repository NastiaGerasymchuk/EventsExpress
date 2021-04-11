﻿using System;
using System.Linq;
using System.Threading.Tasks;
using EventsExpress.Core.Exceptions;
using EventsExpress.Core.IServices;
using EventsExpress.Core.Notifications;
using EventsExpress.Db.BaseService;
using EventsExpress.Db.EF;
using EventsExpress.Db.Entities;
using EventsExpress.Db.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EventsExpress.Core.Services
{
    public class EventStatusHistoryService : BaseService<EventStatusHistory>, IEventStatusHistoryService
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthService _authService;

        public EventStatusHistoryService(
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor,
            IAuthService authService,
            AppDbContext context)
             : base(context)
        {
            _httpContextAccessor = httpContextAccessor;
            _authService = authService;
            _mediator = mediator;
        }

        public async Task CancelEvent(Guid eventId, string reason)
        {
            var uEvent = Context.Events.Find(eventId);
            if (uEvent == null)
            {
                throw new EventsExpressException("Invalid event id");
            }

            var record = CreateEventStatusRecord(uEvent, reason, EventStatus.Cancelled);
            Insert(record);

            await Context.SaveChangesAsync();
            await _mediator.Publish(new CancelEventMessage(eventId));
        }

        private EventStatusHistory CreateEventStatusRecord(Event e, string reason, EventStatus status)
        {
            var record = new EventStatusHistory
            {
                EventId = e.Id,
                UserId = _authService.GetCurrentUser(_httpContextAccessor.HttpContext.User).Id,
                EventStatus = status,
                Reason = reason,
            };

            return record;
        }

        public EventStatusHistory GetLastRecord(Guid eventId, EventStatus status)
        {
            return Context.EventStatusHistory
                .Where(e => e.EventId == eventId && e.EventStatus == status)
                .LastOrDefault();
        }
    }
}
