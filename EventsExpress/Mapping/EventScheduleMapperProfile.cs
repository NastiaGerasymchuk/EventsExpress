﻿using System.Linq;
using AutoMapper;
using EventsExpress.Core.DTOs;
using EventsExpress.Core.Extensions;
using EventsExpress.Db.Entities;
using EventsExpress.ViewModels;

namespace EventsExpress.Mapping
{
    public class EventScheduleMapperProfile : Profile
    {
        public EventScheduleMapperProfile()
        {
            CreateMap<EventSchedule, EventScheduleDto>()
           .ForMember(dest => dest.Event, opts => opts.MapFrom(src => new EventDto
           {
               Id = src.Event.Id,
               Title = src.Event.Title,
               DateTo = src.Event.DateTo,
               DateFrom = src.Event.DateFrom,
               Owners = src.Event.Owners.Select(x => new User
               {
                   Id = x.UserId,
               }),
               PhotoBytes = src.Event.Photo,
           }));

            CreateMap<EventScheduleDto, EventSchedule>()
                .ForMember(dest => dest.Event, opts => opts.Ignore());

            CreateMap<EventScheduleDto, PreviewEventScheduleViewModel>()
                .ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.Event.Title))
                .ForMember(dest => dest.EventId, opts => opts.MapFrom(src => src.EventId))
                .ForMember(dest => dest.PhotoUrl, opts => opts.MapFrom(src => src.Event.PhotoBytes.Thumb.ToRenderablePictureString()));

            CreateMap<EventScheduleDto, EventScheduleViewModel>()
                .ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.Event.Title))
                .ForMember(dest => dest.EventId, opts => opts.MapFrom(src => src.EventId))
                .ForMember(dest => dest.PhotoUrl, opts => opts.MapFrom(src => src.Event.PhotoBytes.Thumb.ToRenderablePictureString()))
                .ForMember(dest => dest.Owners, opts => opts.MapFrom(src => src.Event.Owners.Select(x => new UserPreviewViewModel
                {
                    Id = x.Id,
                    Username = x.Name,
                })));

            CreateMap<PreviewEventScheduleViewModel, EventScheduleDto>()
                .ForMember(dest => dest.Event, opts => opts.Ignore());

            CreateMap<EventDto, EventScheduleDto>()
                .ForMember(dest => dest.EventId, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.LastRun, opts => opts.MapFrom(src => src.DateTo))
                .ForMember(dest => dest.NextRun, opts => opts.MapFrom(src => DateTimeExtensions.AddDateUnit(src.Periodicity, src.Frequency, src.DateTo)))
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.IsActive, opts => opts.MapFrom(src => true))
                .ForMember(dest => dest.Event, opts => opts.Ignore());
        }
    }
}
