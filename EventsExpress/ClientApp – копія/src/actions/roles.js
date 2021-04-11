import { RoleService } from '../services';

export const getRoles = {
    PENDING: 'ROLES_PENDING',
    SUCCESS: 'ROLES_SUCCESS',
    ERROR: 'ROLES_ERROR',
}


const api_serv = new RoleService();


export default function get_roles() {
    return dispatch => {
        dispatch(setRolesPending(true));

        const res = api_serv.getRoles();

        res.then(response => {
            if (response.error == null) {
                dispatch(setRolesSuccess(response));
            } else {
                dispatch(setRolesError(response.error));
            }
        });
    }
}


function setRolesPending(data) {
    return {
        type: getRoles.PENDING,
        payload: data
    }
}  

function setRolesSuccess(data) {
    return {
        type: getRoles.SUCCESS,
        payload: data
    }
}

export function setRolesError(data) {
    return {
        type: getRoles.ERROR,
        payload: data
    }
} 

