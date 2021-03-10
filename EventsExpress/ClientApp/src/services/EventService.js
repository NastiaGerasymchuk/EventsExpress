import EventsExpressService from './EventsExpressService'

const baseService = new EventsExpressService();

export default class EventService {

    getEvent = id => baseService.getResource(`event/${id}`);

    getAllEvents = filters => baseService.getResourceNew(`event/all${filters}`);

    getAllDrafts = (page) => baseService.getResourceNew(`event/AllDraft/${page}`);
          
    getEvents = (eventIds, page) => baseService.setResource(`event/getEvents?page=${page}`, eventIds);

    setEventTemplate = (data, path) => {
        let file = new FormData();
        if (data.id != null) {
            file.append('Id', data.id);
        }

        if (data.photo != null) {
            file.append('Photo', data.photo.file);
        }

        if (data.isReccurent) {
            file.append('IsReccurent', data.isReccurent);
            file.append('Frequency', data.frequency);
            file.append('Periodicity', data.periodicity);
        }

        if (data.photoId) {
            file.append('PhotoId', data.photoId);
        }

        if (data.location) {
            file.append('Location.Type', data.location.type)
            if (data.location.selectedPos) {
                file.append('Location.Latitude', data.location.selectedPos.lat);
                file.append('Location.Longitude', data.location.selectedPos.lng);
            }
            if (data.location.onlineMeeting) {
                file.append('Location.OnlineMeeting', data.location.onlineMeeting);
            }
        }
       




        file.append('Title', data.title);
        file.append('Description', data.description);
        file.append('User.Id', data.user_id);
        file.append('IsPublic', data.isPublic);
        file.append('MaxParticipants', data.maxParticipants);
        file.append('DateFrom', new Date(data.dateFrom).toDateString());
        file.append('DateTo', new Date(data.dateTo).toDateString());

        if (data.inventories) {
            data.inventories.map((item, key) => {
                file.append(`Inventories[${key}].NeedQuantity`, item.needQuantity);
                file.append(`Inventories[${key}].ItemName`, item.itemName);
                file.append(`Inventories[${key}].UnitOfMeasuring.id`, item.unitOfMeasuring.id);
            });
        }

        let i = 0;
        if (data.categories != null) {
            data.categories.map(x => {
                return file.append(`Categories[${i++}].Id`, x.id);
            });
        }
           
        return baseService.setResourceWithData(path, file);
    }

    setEvent = data => this.setEventTemplate(data, `event/create`);

    setCopyEvent = eventId =>
        baseService.setResourceWithData(`event/CreateNextFromParent/${eventId}`);

    setEventFromParent = async (data) =>
        this.setEventTemplate(data, `event/CreateNextFromParentWithEdit/${data.id}`);
    
    editEvent = async (data) => {
        return this.setEventTemplate(data,`event/${data.id}/edit`)
    }
    publishEvent = (id) => {
       return baseService.setResource(`event/${id}/publish`)
    }

    setEventCancel = async (data) => {
        const res = await baseService.setResource(`EventStatusHistory/${data.EventId}/Cancel`, data);
        return !res.ok
            ? { error: await res.text() }
            : await res.json();
    }

    setUserToEvent = async (data) => {
        const res = await baseService.setResource(`event/${data.eventId}/AddUserToEvent?userId=${data.userId}`);
        return !res.ok
            ? { error: await res.text() }
            : res;
    }

    setUserFromEvent = async (data) => {
        const res = await baseService.setResource(`event/${data.eventId}/DeleteUserFromEvent?userId=${data.userId}`);
        return !res.ok
            ? { error: await res.text() }
            : res;
    }

    setApprovedUser = async (data) => {
        const res = data.buttonAction
            ? await baseService.setResource(`event/${data.eventId}/ApproveVisitor?userId=${data.userId}`)
            : await baseService.setResource(`event/${data.eventId}/DenyVisitor?userId=${data.userId}`);
        return !res.ok
            ? { error: await res.text() }
            : res;
    }

    onDeleteFromOwners = async (data) => {
        const res = await baseService.setResource(`owners/DeleteFromOwners?userId=${data.userId}&eventId=${data.eventId}`);
        return !res.ok
            ? { error: await res.text() }
            : res;
    }

    onPromoteToOwner = async (data) => {
        const res = await baseService.setResource(`owners/PromoteToOwner?userId=${data.userId}&eventId=${data.eventId}`);
        return !res.ok
            ? { error: await res.text() }
            : res;
    }

    setRate = async (data) => {
        const res = await baseService.setResource('event/setrate', {
            rate: Number(data.rate),
            userId: data.userId,
            eventId: data.eventId
        });
        return !res.ok
            ? { error: await res.text() }
            : res;
    }

    getCurrentRate = eventId => baseService.getResource(`event/${eventId}/GetCurrentRate`);

    getAverageRate = eventId => baseService.getResource(`event/${eventId}/GetAverageRate`);

    setEventBlock = async (id) => {
        const res = await baseService.setResource(`Event/${id}/Block`);
        return !res.ok
            ? { error: await res.text() }
            : res;
    }

    setEventUnblock = async (id) => {
        const res = await baseService.setResource(`event/${id}/Unblock`);
        return !res.ok
            ? { error: await res.text() }
            : res;
    }

    getFutureEvents = async (id, page) =>
        baseService.getResourceNew(`event/futureEvents?id=${id}&page=${page}`);

    getPastEvents = (id, page) =>
        baseService.getResourceNew(`event/pastEvents?id=${id}&page=${page}`);

    getEventsToGo = (id, page) =>
        baseService.getResourceNew(`event/EventsToGo?id=${id}&page=${page}`);

    getVisitedEvents = (id, page) =>
        baseService.getResourceNew(`event/visitedEvents?id=${id}&page=${page}`);
}
