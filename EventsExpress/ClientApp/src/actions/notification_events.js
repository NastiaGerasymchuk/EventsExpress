import { EventService } from '../services';
import { setEventError, setEventPending, getEvents } from './event-list';
const api_serv = new EventService();

export default function get_events(eventIds,page=1) {

    return dispatch => {
        dispatch(setEventPending(true));

        const res = api_serv.getEvents(eventIds,page);
        res.then(response => {
            if (response.error == null) {
                dispatch(getEvents(response));
            } else {
                dispatch(setEventError(response.error));
            }
        });
    }
}
