﻿using EventsExpress.Core.DTOs;
using EventsExpress.Core.Extensions;
using EventsExpress.Core.Infrastructure;
using EventsExpress.Core.IServices;
using EventsExpress.Core.Notifications;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventsExpress.Core.NotificationHandlers
{
    public class UnblockedEventHandler : INotificationHandler<UnblockedEventMessage>
    {
        private readonly IEmailService _sender;
        private readonly IUserService _userService;
        private readonly IEventService _eventService;
        

        public UnblockedEventHandler(
            IEmailService sender,
            IUserService userSrv,
            IEventService eventService
           
            )
        {
            _sender = sender;
            _userService = userSrv;
            _eventService = eventService;
           
        }

        public async Task Handle(UnblockedEventMessage notification, CancellationToken cancellationToken)
        {
            try
            {
                var Email = _userService.GetById(notification.UserId).Email;
                var Even = _eventService.EventById(notification.Id);
                var MyUrl = MyHttpContext.AppBaseUrl;

                string link = $"{MyUrl}/event/{notification.Id}/1";

                
                
                await _sender.SendEmailAsync(new EmailDTO
                {
                    Subject = "Your event was Unblocked",
                    RecepientEmail = Email,
                    MessageText = $"Dear {Email}, congratulations, your event was Unblocked! " +
                    $"\"<a href='{link}'>{Even.Title}</>\""
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
