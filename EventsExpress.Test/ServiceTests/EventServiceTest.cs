﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using EventsExpress.Core.DTOs;
using EventsExpress.Core.Exceptions;
using EventsExpress.Core.IServices;
using EventsExpress.Core.Services;
using EventsExpress.Db.Entities;
using EventsExpress.Db.Enums;
using EventsExpress.Test.ServiceTests.TestClasses.Event;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using NetTopologySuite.Geometries;
using NUnit.Framework;

namespace EventsExpress.Test.ServiceTests
{
    [TestFixture]
    internal class EventServiceTest : TestInitializer
    {
        private static Mock<IPhotoService> mockPhotoService;
        private static Mock<ILocationService> mockLocationService;
        private static Mock<IEventScheduleService> mockEventScheduleService;
        private static Mock<IMediator> mockMediator;
        private static Mock<IAuthService> mockAuthService;
        private static Mock<IHttpContextAccessor> httpContextAccessor;

        private EventService service;
        private List<Event> events;
        private EventLocation eventLocationMap;
        private EventLocation eventLocationOnline;
        private Guid userId = Guid.NewGuid();
        private Guid eventId = Guid.NewGuid();
        private Guid eventLocationIdMap = Guid.NewGuid();
        private Guid eventLocationIdOnline = Guid.NewGuid();

        private static LocationDto MapLocationDtoFromEventDto(EventDto eventDto)
        {
            if (eventDto.Type == LocationType.Map)
            {
                return new LocationDto
                {
                    Id = Guid.NewGuid(),
                    Point = eventDto.Point,
                    Type = LocationType.Map,
                };
            }
            else if (eventDto.Type == LocationType.Online)
            {
                return new LocationDto
                {
                    Id = Guid.NewGuid(),
                    OnlineMeeting = eventDto.OnlineMeeting,
                    Type = LocationType.Online,
                };
            }

            return null;
        }

        private static EventLocation MapEventLocationFromLocationDto(LocationDto locationDto)
        {
            if (locationDto.Type == LocationType.Map)
            {
                return new EventLocation
                {
                    Id = Guid.NewGuid(),
                    Point = locationDto.Point,
                    Type = LocationType.Map,
                };
            }
            else if (locationDto.Type == LocationType.Online)
            {
                return new EventLocation
                {
                    Id = Guid.NewGuid(),
                    OnlineMeeting = locationDto.OnlineMeeting,
                    Type = LocationType.Online,
                };
            }

            return null;
        }

        private EventDto DeepCopyDto(EventDto eventDto)
        {
            List<User> users = new List<User>();
            foreach (var owner in eventDto.Owners)
            {
                users.Add(owner);
            }

            return new EventDto
            {
                Id = eventDto.Id,
                DateFrom = eventDto.DateFrom,
                DateTo = eventDto.DateTo,
                Description = eventDto.Description,
                Owners = users,
                PhotoId = eventDto.PhotoId,
                Title = eventDto.Title,
                IsBlocked = eventDto.IsBlocked,
                IsPublic = eventDto.IsPublic,
                Categories = eventDto.Categories,
                Point = eventDto.Point,
                MaxParticipants = eventDto.MaxParticipants,
                Type = eventDto.Type,
            };
        }

        [SetUp]
        protected override void Initialize()
        {
            base.Initialize();
            mockMediator = new Mock<IMediator>();
            mockPhotoService = new Mock<IPhotoService>();
            mockLocationService = new Mock<ILocationService>();
            mockEventScheduleService = new Mock<IEventScheduleService>();
            httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.SetupGet(x => x.HttpContext)
                .Returns(new Mock<HttpContext>().Object);
            mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(x => x.GetCurrentUser(It.IsAny<ClaimsPrincipal>()))
                .Returns(new UserDto { Id = userId });

            service = new EventService(
                Context,
                MockMapper.Object,
                mockMediator.Object,
                mockPhotoService.Object,
                mockLocationService.Object,
                mockAuthService.Object,
                httpContextAccessor.Object,
                mockEventScheduleService.Object);

            eventLocationMap = new EventLocation
            {
                Id = eventLocationIdMap,
                Point = new Point(10.45, 12.34),
                Type = LocationType.Map,
            };
            eventLocationOnline = new EventLocation
            {
                Id = eventLocationIdOnline,
                OnlineMeeting = new Uri("http://basin.example.com/#branch"),
                Type = LocationType.Online,
            };

            List<User> users = new List<User>()
            {
                new User
                {
                    Id = userId,
                    Name = "NameIsExist",
                    Email = "stas@gmail.com",
                },
            };

            events = new List<Event>
            {
                new Event
                {
                    Id = GetEventExistingId.FirstEventId,
                    DateFrom = DateTime.Today,
                    DateTo = DateTime.Today,
                    Description = "sjsdnl sdmkskdl dsnlndsl",
                    Owners = new List<EventOwner>()
                    {
                        new EventOwner
                        {
                            UserId = Guid.NewGuid(),
                        },
                    },
                    PhotoId = Guid.NewGuid(),
                    EventLocationId = eventLocationIdMap,
                    Title = "SLdndsndj",
                    IsBlocked = false,
                    IsPublic = true,
                    Categories = null,
                    MaxParticipants = 2147483647,
                },
                new Event
                {
                    Id = GetEventExistingId.SecondEventId,
                    DateFrom = DateTime.Today,
                    DateTo = DateTime.Today,
                    Description = "sjsdnl sdmkskdl dsnlndsl",
                    Owners = new List<EventOwner>()
                    {
                        new EventOwner
                        {
                            UserId = Guid.NewGuid(),
                        },
                    },
                    PhotoId = Guid.NewGuid(),
                    EventLocationId = eventLocationIdOnline,
                    Title = "SLdndsndj",
                    IsBlocked = false,
                    IsPublic = true,
                    Categories = null,
                    MaxParticipants = 2147483647,
                },
                new Event
                {
                    Id = eventId,
                    DateFrom = DateTime.Today,
                    DateTo = DateTime.Today,
                    Description = "sjsdnl fgr sdmkskdl dsnlndsl",
                    Owners = new List<EventOwner>()
                    {
                        new EventOwner
                        {
                            UserId = Guid.NewGuid(),
                        },
                    },
                    PhotoId = Guid.NewGuid(),
                    Title = "SLdndstrhndj",
                    IsBlocked = false,
                    IsPublic = false,
                    Categories = null,
                    MaxParticipants = 1,
                    Visitors = new List<UserEvent>()
                    {
                        new UserEvent
                        {
                            UserStatusEvent = UserStatusEvent.Pending,
                            Status = Status.WillGo,
                            UserId = userId,
                            User = users[0],
                            EventId = eventId,
                        },
                    },
                },
            };

            Context.EventLocations.Add(eventLocationMap);
            Context.EventLocations.Add(eventLocationOnline);
            Context.Events.AddRange(events);
            Context.SaveChanges();

            MockMapper.Setup(u => u.Map<EventDto, LocationDto>(It.IsAny<EventDto>()))
                .Returns((EventDto e) => MapLocationDtoFromEventDto(e));

            MockMapper.Setup(u => u.Map<LocationDto, EventLocation>(It.IsAny<LocationDto>()))
                .Returns((LocationDto e) => MapEventLocationFromLocationDto(e));

            MockMapper.Setup(u => u.Map<EventDto, Event>(It.IsAny<EventDto>()))
                .Returns((EventDto e) => e == null ?
                null :
                new Event
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    PhotoId = e.PhotoId,
                    DateFrom = e.DateFrom,
                    DateTo = e.DateTo,
                    MaxParticipants = e.MaxParticipants,
                });

            MockMapper.Setup(u => u.Map<EventDto>(It.IsAny<Event>()))
                .Returns((Event e) => e == null ?
                null :
                new EventDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    PhotoId = (Guid)e.PhotoId,
                    DateFrom = e.DateFrom,
                    DateTo = e.DateTo,
                    MaxParticipants = e.MaxParticipants,
                });
        }

        [TestCaseSource(typeof(GetEventExistingId))]
        [Category("Get Event")]
        public void GetEvent_ExistingId_Success(Guid existingId)
        {
            Assert.DoesNotThrow(() => service.EventById(existingId));
        }

        [Test]
        [Category("Get Event")]
        public void GetEvent_NotExistingId_Failed()
        {
            var result = service.EventById(Guid.NewGuid());

            Assert.IsNull(result);
        }

        [TestCaseSource(typeof(EditingOrCreatingExistingDto))]
        [Category("Create Event")]
        public void CreateEvent_ValidEvent_Success(EventDto eventDto)
        {
            EventDto dto = DeepCopyDto(eventDto);
            dto.Id = Guid.Empty;

            Assert.DoesNotThrowAsync(async () => await service.Create(dto));
        }

        [TestCaseSource(typeof(EditingOrCreatingExistingDto))]
        [Category("Edit Event")]
        public void EditEvent_ValidEvent_Success(EventDto eventDto)
        {
            Assert.DoesNotThrowAsync(async () => await service.Edit(eventDto));
        }

        [Test]
        [Category("Edit Event")]
        public void EditEvent_InvalidEvent_Failed()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () => await service.Edit(null));
        }

        [TestCaseSource(typeof(GetEventExistingId))]
        [Category("Add user to event")]
        public void AddUserToEvent_ReturnTrue(Guid id)
        {
            Assert.DoesNotThrowAsync(async () => await service.AddUserToEvent(userId, id));
        }

        [Test]
        [Category("Add user to event")]
        public void AddUserToEvent_UserNotFound_ReturnFalse()
        {
            Assert.ThrowsAsync<EventsExpressException>(async () => await service.AddUserToEvent(Guid.NewGuid(), eventId));
        }

        [Test]
        [Category("Add user to event")]
        public void AddUserToEvent_ToMuchParticipants_ReturnFalse()
        {
            Assert.ThrowsAsync<EventsExpressException>(async () => await service.AddUserToEvent(userId, eventId));
        }

        [Test]
        [Category("Add user to event")]
        public void AddUserToEvent_EventNotFound_ReturnFalse()
        {
            Assert.ThrowsAsync<EventsExpressException>(async () => await service.AddUserToEvent(userId, Guid.NewGuid()));
        }

        [Test]
        [Category("Delete user")]
        public void DeleteUserFromEvent_ReturnTrue()
        {
            Assert.DoesNotThrowAsync(async () => await service.DeleteUserFromEvent(userId, eventId));
        }

        [Test]
        [Category("Delete user")]
        public void DeleteUserFromEvent_UserNotFound_ReturnFalse()
        {
            Assert.ThrowsAsync<EventsExpressException>(async () => await service.DeleteUserFromEvent(Guid.NewGuid(), eventId));
        }

        [Test]
        [Category("Delete user")]
        public void DeleteUserFromEvent_EventNotFound_ReturnFalse()
        {
            Assert.ThrowsAsync<EventsExpressException>(async () => await service.DeleteUserFromEvent(userId, Guid.NewGuid()));
        }

        [Test]
        [Category("Change visitor status")]
        public void ChangeVisitorStatus_ReturnTrue()
        {
            Assert.DoesNotThrowAsync(async () => await service.ChangeVisitorStatus(
                userId,
                eventId,
                UserStatusEvent.Approved));
        }

        [Test]
        [Category("GetAll")]
        public void GetAll_ExistingPoint_True()
        {
            EventFilterViewModel eventFilterViewModel = new EventFilterViewModel { Page = 1, PageSize = 8, X = 10.45, Y = 12.34, Radius = 10 };
            int count = 8;
            var res = service.GetAll(eventFilterViewModel, out count);
        }
    }
}
