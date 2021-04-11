﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EventsExpress.Core.DTOs;
using EventsExpress.Core.IServices;
using EventsExpress.Db.Enums;
using EventsExpress.Filters;
using EventsExpress.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsExpress.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public EventController(
            IEventService eventService,
            IAuthService authSrv,
            IMapper mapper)
        {
            _eventService = eventService;
            _authService = authSrv;
            _mapper = mapper;
        }

        /// <summary>
        /// This method is for edit event from event schedule and create it.
        /// </summary>
        /// <param name="eventId">Param eventId defines the event identifier.</param>
        /// <param name="model">Param model provides access to event's properties.</param>
        /// <returns>The method returns an identifier of created event.</returns>
        /// <response code="200">Create event proces success.</response>
        /// <response code="400">If Create process failed.</response>
        [HttpPost("[action]/{eventId:Guid}")]
        [UserAccessTypeFilterAttribute]
        public async Task<IActionResult> CreateNextFromParentWithEdit(Guid eventId, [FromForm] EventEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _eventService.EditNextEvent(_mapper.Map<EventDto>(model));

            return Ok(new { id = result });
        }

        /// <summary>
        /// This method is for create event from event schedule.
        /// </summary>
        /// <param name="eventId">Param eventId defines the event identifier.</param>
        /// <returns>The method returns an identifier of created event.</returns>
        /// <response code="200">Create event proces success.</response>
        /// <response code="400">If Create process failed.</response>
        [HttpPost("[action]/{eventId:Guid}")]
        public async Task<IActionResult> CreateNextFromParent(Guid eventId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _eventService.CreateNextEvent(eventId);

            return Ok(new { id = result });
        }

        /// <summary>
        /// This method is for edit and create events.
        /// </summary>
        /// <param name="model">Param model provides access to event's properties.</param>
        /// <returns>The method returns a created event.</returns>
        /// <response code="200">Create event proces success.</response>
        /// <response code="400">If Create process failed.</response>
        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromForm] EventCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _eventService.Create(_mapper.Map<EventDto>(model));

            return Ok(result);
        }

        /// <summary>
        /// This method is for edit and create events.
        /// </summary>
        /// <param name="eventId">Param eventId defines the event identifier.</param>
        /// <param name="model">Param model provides access to event's properties.</param>
        /// <returns>The method returns an edited event.</returns>
        /// <response code="200">Edit event proces success.</response>
        /// <response code="400">If Edit process failed.</response>
        [HttpPost("{eventId:Guid}/[action]")]
        [UserAccessTypeFilterAttribute]
        public async Task<IActionResult> Edit(Guid eventId, [FromForm] EventEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _eventService.Edit(_mapper.Map<EventDto>(model));

            return Ok(result);
        }

        /// <summary>
        /// This method have to return event.
        /// </summary>
        /// <param name="eventId">Param eventId defines the event identifier.</param>
        /// <returns>The method returns an event by identifier.</returns>
        /// <response code="200">Return UserInfo model.</response>
        [AllowAnonymous]
        [HttpGet("{eventId:Guid}")]
        public IActionResult Get(Guid eventId) =>
            Ok(_mapper.Map<EventViewModel>(_eventService.EventById(eventId)));

        /// <summary>
        /// This method have to return all events.
        /// </summary>
        /// <param name="filter">Param filter provides the ability to filter the list of events.</param>
        /// <returns>The method returns filltered events.</returns>
        /// <response code="200">Return IEnumerable EventPreviewDto.</response>
        /// <response code="400">If return failed.</response>
        [AllowAnonymous]
        [HttpGet("[action]")]
        public IActionResult All([FromQuery] EventFilterViewModel filter)
        {
            filter.PageSize = 6;
            filter.OwnerId = null;
            filter.VisitorId = null;

            if (!User.IsInRole("Admin"))
            {
                if (filter.DateFrom == DateTime.MinValue)
                {
                    filter.DateFrom = DateTime.Today;
                }

                if (filter.Status != EventStatus.Active)
                {
                    return Forbid();
                }
            }

            try
            {
                var viewModel = new IndexViewModel<EventPreviewViewModel>
                {
                    Items = _mapper.Map<IEnumerable<EventPreviewViewModel>>(
                        _eventService.GetAll(filter, out int count)),
                    PageViewModel = new PageViewModel(count, filter.Page, filter.PageSize),
                };
                return Ok(viewModel);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// This method have to add user to event.
        /// </summary>
        /// <param name="eventId">Param eventId defines the event identifier.</param>
        /// <param name="userId">Param userId defines the user identifier.</param>
        /// <returns>The method returns new participant of the event.</returns>
        /// <response code="200">Adding user from event proces success.</response>
        /// <response code="400">If adding user from event process failed.</response>
        [HttpPost("{eventId:Guid}/[action]")]
        public async Task<IActionResult> AddUserToEvent(Guid eventId, Guid userId)
        {
            await _eventService.AddUserToEvent(userId, eventId);

            return Ok();
        }

        /// <summary>
        /// This method have to approved user on event.
        /// </summary>
        /// <param name="eventId">Param eventId defines the event identifier.</param>
        /// <param name="userId">Param userId defines the user identifier.</param>
        /// <returns>The method returns approved participant.</returns>
        /// <response code="200">Approving user from event process success.</response>
        /// <response code="400">If aproving user from event process failed.</response>
        [HttpPost("{eventId:Guid}/[action]")]
        [UserAccessTypeFilterAttribute]
        public async Task<ActionResult> ApproveVisitor(Guid eventId, Guid userId)
        {
            await _eventService.ChangeVisitorStatus(userId, eventId, UserStatusEvent.Approved);

            return Ok();
        }

        /// <summary>
        /// This method have to denied participation in event.
        /// </summary>
        /// <param name="eventId">Param eventId defines the event identifier.</param>
        /// <param name="userId">Param userId defines the user identifier.</param>
        /// <returns>The method returns denied participant.</returns>
        /// <response code="200">Denying user from event process success.</response>
        /// <response code="400">If denying user from event process failed.</response>
        [HttpPost("{eventId:Guid}/[action]")]
        [UserAccessTypeFilterAttribute]
        public async Task DenyVisitor(Guid eventId, Guid userId)
        {
            await _eventService.ChangeVisitorStatus(userId, eventId, UserStatusEvent.Denied);
        }

        /// <summary>
        /// This method have to add user to category.
        /// </summary>
        /// <param name="eventId">Param eventId defines the event identifier.</param>
        /// <param name="userId">Param userId defines the user identifier.</param>
        /// <returns>The method returns deleted participant.</returns>
        /// <response code="200">Delete  user from event proces success.</response>
        /// <response code="400">If deleting user from event process failed.</response>
        [HttpPost("{eventId:Guid}/[action]")]
        public async Task<IActionResult> DeleteUserFromEvent(Guid eventId, Guid userId)
        {
            await _eventService.DeleteUserFromEvent(userId, eventId);

            return Ok();
        }

        /// <summary>
        /// This method is to block event.
        /// </summary>
        /// <param name="eventId">Param eventId defines the event identifier.</param>
        /// <returns>The method returns blocked event.</returns>
        /// <response code="200">Block is succesful.</response>
        /// <response code="302">If user isn't admin.</response>
        /// <response code="400">Block process failed.</response>
        [HttpPost("{eventId:Guid}/[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Block(Guid eventId)
        {
            await _eventService.BlockEvent(eventId);

            return Ok();
        }

        /// <summary>
        /// This method is to unblock event.
        /// </summary>
        /// <param name="eventId">Param eventId defines the event identifier.</param>
        /// <returns>The method returns unblocked event.</returns>
        /// <response code="200">Unblock is succesful.</response>
        /// <response code="400">Unblock process is failed.</response>
        [HttpPost("{eventId:Guid}/[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Unblock(Guid eventId)
        {
            await _eventService.UnblockEvent(eventId);

            return Ok();
        }

        /// <summary>
        /// This method id used to set rating to user.
        /// </summary>
        /// <param name="model">Param model provides access to rate's properties.</param>
        /// <returns>The method returns rate of the event.</returns>
        /// <response code="200">Rating is setted successfully.</response>
        /// <response code="400">Setting rating is failed.</response>
        [HttpPost("[action]")]
        public async Task<IActionResult> SetRate(RateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _eventService.SetRate(model.UserId, model.EventId, model.Rate);

            return Ok();
        }

        /// <summary>
        /// This method gets current rate for event.
        /// </summary>
        /// <param name="eventId">Param eventId defines the event identifier.</param>
        /// <returns>The method returns current rate for event.</returns>
        /// <response code="200">Getting is successful.</response>
        /// <response code="400">Getting is failed.</response>
        [HttpGet("{eventId:Guid}/[action]")]
        public IActionResult GetCurrentRate(Guid eventId)
        {
            if (!_eventService.Exists(eventId))
            {
                return BadRequest("Invalid id");
            }

            var userId = _authService.GetCurrentUser(HttpContext.User).Id;

            return Ok(_eventService.GetRateFromUser(userId, eventId));
        }

        /// <summary>
        /// This method gets average rate for event.
        /// </summary>
        /// <param name="eventId">Param eventId defines the event identifier.</param>
        /// <returns>The method returns average rate for event.</returns>
        /// <response code="200">Getting is successful.</response>
        /// <response code="400">Getting is failed.</response>
        [HttpGet("{eventId:Guid}/[action]")]
        public IActionResult GetAverageRate(Guid eventId)
        {
            if (!_eventService.Exists(eventId))
            {
                return BadRequest("Invalid id");
            }

            return Ok(_eventService.GetRate(eventId));
        }

        /// <summary>
        /// This method gets future events for user profile.
        /// </summary>
        /// <param name="id">Param id defines the user identifier.</param>
        /// <param name="page">Param page defines page for comments.</param>
        /// <returns>The method returns future events.</returns>
        /// <response code="200">Getting is successful.</response>
        /// <response code="400">Getting is failed.</response>
        [HttpGet("[action]")]
        public IActionResult FutureEvents(Guid id, int page = 1)
        {
            var model = new PaginationViewModel
            {
                PageSize = 3,
                Page = page,
            };
            try
            {
                var viewModel = new IndexViewModel<EventPreviewViewModel>
                {
                    Items = _mapper.Map<IEnumerable<EventPreviewViewModel>>(_eventService.FutureEventsByUserId(id, model)),
                    PageViewModel = new PageViewModel(model.Count, model.Page, model.PageSize),
                };
                return Ok(viewModel);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// This method gets finished events.
        /// </summary>
        /// <param name="id">Param id defines the user identifier.</param>
        /// <param name="page">Param page defines page for comments.</param>
        /// <returns>The method returns past events.</returns>
        /// <response code="200">Getting is successful.</response>
        /// <response code="400">Getting is failed.</response>
        [HttpGet("[action]")]
        public IActionResult PastEvents(Guid id, int page = 1)
        {
            var model = new PaginationViewModel
            {
                PageSize = 3,
                Page = page,
            };
            try
            {
                var viewModel = new IndexViewModel<EventPreviewViewModel>
                {
                    Items = _mapper.Map<IEnumerable<EventPreviewViewModel>>(_eventService.PastEventsByUserId(id, model)),
                    PageViewModel = new PageViewModel(model.Count, model.Page, model.PageSize),
                };
                return Ok(viewModel);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// This method gets  events which have to visit.
        /// </summary>
        /// <param name="id">Param id defines the user identifier.</param>
        /// <param name="page">Param page defines page for comments.</param>
        /// <returns>The method returns events to go.</returns>
        /// <response code="200">Getting is successful.</response>
        /// <response code="400">Getting is failed.</response>
        [HttpGet("[action]")]
        public IActionResult EventsToGo(Guid id, int page = 1)
        {
            var model = new PaginationViewModel
            {
                PageSize = 3,
                Page = page,
            };
            try
            {
                var viewModel = new IndexViewModel<EventPreviewViewModel>
                {
                    Items = _mapper.Map<IEnumerable<EventPreviewViewModel>>(_eventService.EventsToGoByUserId(id, model)),
                    PageViewModel = new PageViewModel(model.Count, model.Page, model.PageSize),
                };
                return Ok(viewModel);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// This method gets  events which have visited.
        /// </summary>
        /// <param name="id">Param id defines the user identifier.</param>
        /// <param name="page">Param page defines page for comments.</param>
        /// <returns>The method returns visited events.</returns>
        /// <response code="200">Getting is successful.</response>
        /// <response code="400">Getting is failed.</response>
        [HttpGet("[action]")]
        public IActionResult VisitedEvents(Guid id, int page = 1)
        {
            var model = new PaginationViewModel
            {
                PageSize = 3,
                Page = page,
            };
            try
            {
                var viewModel = new IndexViewModel<EventPreviewViewModel>
                {
                    Items = _mapper.Map<IEnumerable<EventPreviewViewModel>>(_eventService.VisitedEventsByUserId(id, model)),
                    PageViewModel = new PageViewModel(model.Count, model.Page, model.PageSize),
                };
                return Ok(viewModel);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// This method gets  events. Used for notifications.
        /// </summary>
        /// <param name="eventIds">Param eventIds defines identifiers of the events.</param>
        /// <param name="page">Param page defines page for comments.</param>
        /// <returns>The method returns events by identifiers.</returns>
        /// <response code="200">Getting is successful.</response>
        /// <response code="400">Getting is failed.</response>
        [HttpPost("[action]")]
        public IActionResult GetEvents([FromBody] List<Guid> eventIds, [FromQuery] int page = 1)
        {
            var model = new PaginationViewModel
            {
                PageSize = 1,
                Page = page,
            };

            try
            {
                var viewModel = new IndexViewModel<EventPreviewViewModel>
                {
                    Items = _mapper.Map<IEnumerable<EventPreviewViewModel>>(
                        _eventService.GetEvents(eventIds, model)),
                    PageViewModel = new PageViewModel(model.Count, model.Page, model.PageSize),
                };
                return Ok(viewModel);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
        }
    }
}
