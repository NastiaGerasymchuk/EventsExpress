
import initialState from '../store/initialState';
import {
    GET_EVENT_SCHEDULE_ERROR, GET_EVENT_SCHEDULE_PENDING, GET_EVENT_SCHEDULE_SUCCESS, RESET_EVENT_SCHEDULE
} from '../actions/eventSchedule-item-view';


export const reducer = (
    state = initialState.eventSchedule,
    action
) => {
    switch (action.type) {
        case GET_EVENT_SCHEDULE_ERROR:
            return {
                ...state,
                isPending: false,
                isError: action.payload
            }
        case GET_EVENT_SCHEDULE_PENDING:
            return {
                ...state,
                isPending: action.payload
            }
        case GET_EVENT_SCHEDULE_SUCCESS:
            return {
                ...state,
                isPending: false,
                data: action.payload
            }
        case RESET_EVENT_SCHEDULE:
            return {
                ...initialState.eventSchedule
            }
        default:
            return state;
    }
}  