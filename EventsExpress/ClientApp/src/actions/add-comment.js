import { CommentService } from '../services';
import get_comments from './comment-list';
import { SubmissionError } from 'redux-form'
import { buildValidationState } from '../components/helpers/helpers.js'

export const SET_COMMENT_PENDING = "SET_COMMENT_PENDING";
export const SET_COMMENT_SUCCESS = "SET_COMMENT_SUCCESS";

const api_serv = new CommentService();

export default function add_comment(data) {
    return dispatch => {
        dispatch(setCommentPending(true));

        return api_serv.setComment(data)
            .then(response => {
                if (response.error != null) {
                    throw new SubmissionError(buildValidationState(response.error));
                }

                dispatch(setCommentSuccess(true));
                dispatch(get_comments(data.eventId, 1));
                return Promise.resolve();
            });
    }
}

export function setCommentSuccess(data) {
    return {
        type: SET_COMMENT_SUCCESS,
        payload: data
    };
}

export function setCommentPending(data) {
    return {
        type: SET_COMMENT_PENDING,
        payload: data
    };
}