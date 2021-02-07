import { EventService } from '../services';
import { setAlert } from './alert';
import {createBrowserHistory} from 'history';

export const SET_COPY_EVENT_SUCCESS = "SET_COPY_EVENT_SUCCESS";
export const SET_COPY_EVENT_PENDING = "SET_COPY_EVENT_PENDING";
export const SET_COPY_EVENT_ERROR = "SET_COPY_EVENT_ERROR";
export const EVENT_COPY_WAS_CREATED = "EVENT_COPY_WAS_CREATED";

const api_serv = new EventService();
const history = createBrowserHistory({forceRefresh:true});

export default function add_copy_event(eventId) {
    return dispatch => {
        dispatch(setCopyEventPending(true));

        return api_serv.setCopyEvent(eventId)
            .then(response => {
                if (response.error != null) {
                    let alertObj = buildAllertWithError(buildValidationState(response.error))
                    
                    dispatch(setAlert(buildAllertWithError(buildValidationState(response.error))));
                    return Promise.reject();
                }

                dispatch(setCopyEventSuccess(true));
                return response.json()
                    .then(x => { 
                        dispatch(copyEventWasCreated(x));
                        dispatch(setAlert(buildAllertWithSuccess('Your event was created!')));
                        dispatch(history.push(`/event/${x.id}/1`));
                        return Promise.resolve();
                    });
            });
    }
}


let buildAllertWithError = (msg) => ({
    variant: 'error', 
    message: msg,
})

let buildAllertWithSuccess = (msg) => ({
    variant: 'success', 
    message: msg,
})

function copyEventWasCreated(eventId) {
    return {
        type: EVENT_COPY_WAS_CREATED,
        payload: eventId
    }
}

export function setCopyEventSuccess(eventId) {
    return {
        type: SET_COPY_EVENT_SUCCESS,
        payload: eventId
    };
}

export function setCopyEventPending(eventId) {
    return {
        type: SET_COPY_EVENT_PENDING,
        payload: eventId
    };
}

export function setCopyEventError(eventId) {
    return {
        type: SET_COPY_EVENT_ERROR,
        payload: eventId
    };
}

