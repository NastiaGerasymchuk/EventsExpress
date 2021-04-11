﻿import { UserService } from '../../services';
import { setAlert } from '../alert';

export const editBirthday = {
    PENDING: "SET_EDITBIRTHDAY_PENDING",
    SUCCESS: "SET_EDITBIRTHDAY_SUCCESS",
    ERROR: "SET_EDITBIRTHDAY_ERROR",
    UPDATE: "UPDATE_BIRTHDAY"
}

const api_serv = new UserService();

export default function edit_Birthday(data) {
    return dispatch => {
        dispatch(setEditBirthdayPending(true));
        const res = api_serv.setBirthday(data);
        res.then(response => {
            if (response.error == null) {
                dispatch(setEditBirthdaySuccess(true));
                dispatch(updateBirthday(data.Birthday));
                dispatch(setAlert({ variant: 'success', message: 'Set date of birth successed' }));
            } else {
                dispatch(setEditBirthdayError(response.error));
                dispatch(setAlert({ variant: 'error', message: 'Failed' }));
            }
        });
    }
}

function updateBirthday(data) {
    return {
        type: editBirthday.UPDATE,
        payload: data
    };
}

function setEditBirthdayPending(data) {
    return {
        type: editBirthday.PENDING,
        payload: data
    };
}

function setEditBirthdaySuccess(data) {
    return {
        type: editBirthday.SUCCESS,
        payload: data
    };
}

export function setEditBirthdayError(data) {
    return {
        type: editBirthday.ERROR,
        payload: data
    };
}
