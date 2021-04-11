import React, { Component } from 'react';
import EventForm from '../components/event/event-form';
import edit_event_from_parent from '../actions/edit-event-from-parent';
import { connect } from 'react-redux';
import { setAlert } from '../actions/alert';
import { reset } from 'redux-form';
import {
    setEventFromParentError,
    setEventFromParentPending,
    setEventFromParentSuccess
}
    from '../actions/edit-event-from-parent';
import * as moment from 'moment';
import { validateEventForm } from '../components/helpers/helpers'
import get_categories from '../actions/category/category-list';

class EditFromParentEventWraper extends Component {

    componentWillMount = () => {
        this.props.get_categories();
    }

    componentDidUpdate = () => {
        if (!this.props.edit_event_from_parent_status.eventFromParentError &&
            this.props.edit_event_from_parent_status.isEventFromParentSuccess) {
            this.props.reset();
        }
    }

    componentWillUnmount() {
        this.props.reset();
    }

    onSubmit = (values) => {
        if (values.isReccurent) {
            values.isReccurent = false;
        }
        this.props.edit_event_from_parent({ ...validateEventForm(values), user_id: this.props.user_id });
    }

    render() {
        let initialValues = {
            ...this.props.event,
            dateFrom: this.props.eventSchedule.nextRun,
            dateTo: new moment(this.props.event.dateTo)
                .add(new moment(this.props.event.nextRun)
                    .diff(new moment(this.props.event.dateFrom), 'days'), 'days')
        }

        return <>
            <EventForm
                all_categories={this.props.all_categories}
                cities={this.props.cities.data}
                onChangeCountry={this.onChangeCountry}
                onCancel={this.props.onCancelEditing}
                onSubmit={this.onSubmit}
                countries={this.props.countries.data}
                initialValues={initialValues}
                haveReccurentCheckBox={false}
                haveMapCheckBox={true}
                haveOnlineLocationCheckBox={true}
                disabledDate={true}
                isCreated={true} />
        </>
    }
}

const mapStateToProps = (state) => ({
    user_id: state.user.id,
    edit_event_from_parent_status: state.edit_event_from_parent,
    all_categories: state.categories.data,
    eventSchedule: state.eventSchedule.data,
    event: state.event.data,
});

const mapDispatchToProps = (dispatch) => {
    return {
        edit_event_from_parent: (data) => dispatch(edit_event_from_parent(data)),
        get_categories: () => dispatch(get_categories()),
        resetEvent: () => dispatch(reset('event-form')),
        alert: (data) => dispatch(setAlert(data)),
        reset: () => {
            dispatch(setEventFromParentPending(true));
            dispatch(setEventFromParentSuccess(false));
            dispatch(setEventFromParentError(null));
        }
    }
};

export default connect(mapStateToProps, mapDispatchToProps)(EditFromParentEventWraper);