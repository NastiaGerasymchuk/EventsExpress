import React, { Component } from "react";
import { connect } from "react-redux";
import IconButton from "@material-ui/core/IconButton";
import UnitOfMeasuringItem from "../../components/unitOfMeasuring/unitOfMeasuring-item";
import UnitOfMeasuringEdit from "../../components/unitOfMeasuring/unitOfMeasuring-edit";
import { add_unitOfMeasuring } from "../../actions/unitOfMeasuring/add-unitOfMeasuring";
import { delete_unitOfMeasuring } from "../../actions/unitOfMeasuring/delete-unitOfMeasuring";
import { set_edited_unitOfMeasuring } from "../../actions/unitOfMeasuring/add-unitOfMeasuring";
import { confirmAlert } from 'react-confirm-alert';
import 'react-confirm-alert/src/react-confirm-alert.css';

class UnitOfMeasuringItemWrapper extends Component {
    save = values => {
        if (values.unitName === this.props.item.unitName &&
            values.shortName === this.props.item.shortName) {
            this.props.edit_cancel();
        } else {
            this.props.save_unitOfMeasuring({ ...values, id: this.props.item.id });
        }
    };

    componentWillUpdate = () => {
        const { unitOfMeasuringError, isUnitOfMeasuringSuccess } = this.props.status;

        if (!unitOfMeasuringError && isUnitOfMeasuringSuccess) {
            this.props.edit_cancel();
        }
    }

    isDeleteConfirm = () => {
        const { unitName, shortName, id } = this.props.item;
        confirmAlert({
            title: 'Do you really want to remove this Unit Of Measuring?',
            message: <div>
                Unit name is {unitName}<br />
            Short name is {shortName}
            </div>,
            buttons: [
                {
                    label: 'Yes',
                    onClick: () => { this.props.delete_unitOfMeasuring(id); }
                },
                {
                    label: 'No',
                }
            ]
        });

    }
    render() {
        const { set_unitOfMeasuring_edited, edit_cancel } = this.props;

        return <tr>
            {(this.props.item.id === this.props.editedUnitOfMeasuring)
                ? <UnitOfMeasuringEdit
                    key={this.props.item.id + this.props.editedUnitOfMeasuring}
                    item={this.props.item}
                    callback={this.save}
                    cancel={edit_cancel}
                    message={this.props.status.unitOfMeasuringError}
                />
                : <UnitOfMeasuringItem
                    item={this.props.item}
                    callback={set_unitOfMeasuring_edited}
                />
            }
            <td className="align-middle align-items-stretch">
                <div className="d-flex align-items-center justify-content-center" width="15%">
                    <IconButton className="text-danger" size="small" onClick={this.isDeleteConfirm}>
                        <i className="fas fa-trash"></i>
                    </IconButton>
                </div>
            </td>

        </tr>
    }
}

const mapStateToProps = state => {
    return {
        status: state.add_unitOfMeasuring,
        editedUnitOfMeasuring: state.unitsOfMeasuring.editedUnitOfMeasuring
    }

};

const mapDispatchToProps = (dispatch, props) => {
    return {
        delete_unitOfMeasuring: () => dispatch(delete_unitOfMeasuring(props.item.id)),
        save_unitOfMeasuring: (data) => dispatch(add_unitOfMeasuring(data)),
        set_unitOfMeasuring_edited: () => dispatch(set_edited_unitOfMeasuring(props.item.id)),
        edit_cancel: () => dispatch(set_edited_unitOfMeasuring(null))
    };
};

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(UnitOfMeasuringItemWrapper);