﻿import React, { Component } from 'react';
import Button from '@material-ui/core/Button';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogTitle from '@material-ui/core/DialogTitle';
import LocationMap from './map/location-map';
import { Field } from 'redux-form';
import eventHelper from '../helpers/eventHelper';
import './slider.css';
import DisplayMap from '../event/map/display-map';
class MapModal extends Component {
    constructor(props) {
        super(props);
        this.state = {
            open: false,
            selectedPos: null,
            radius: this.props.initialValues.radius != undefined ? this.props.initialValues.radius : 0,
            needInitializeValues: true,
            map: false
        }
        this.baseState = this.state;
        this.radius = React.createRef();
    }
    componentDidUpdate(prevProps) {
        const initialValues = this.props.initialFormValues;
        if (!eventHelper.compareObjects(initialValues, prevProps.initialFormValues)
            || this.state.needInitializeValues) {
            this.props.initialize({
                radius: this.props.values.radius,
                selectedPos: this.props.values.selectedPos
            });
            this.setState({
                ['needInitializeValues']: false
            });
        }
    }

    componentDidUpdate() {
        return true;
    }

    handleClickOpen = () => {
        this.setState({ open: true });
    };
    handleClose = () => {
        console.log(this.props)
        this.props.initialize({
            radius: this.props.initialValues.radius,
            selectedPos: this.props.initialValues.selectedPos != null ? this.props.initialValues.selectedPos : { lat: null, lng: null },
            dateFrom:this.props.values.dateFrom!=undefined?this.props.values.dateFrom:null,
            dateTo:this.props.values.dateTo!=undefined?this.props.values.dateTo:null,
            categories:this.props.values.categories!=undefined?this.props.values.categories:[],
            keyWord:this.props.values.keyWord!=undefined?this.props.values.keyWord:null
        })
    };
    handleClear = () => {
        console.log(this.props)
        this.props.initialize({
            radius: 8,
            selectedPos:{ lat: null, lng: null },
            dateFrom:this.props.values.dateFrom!=undefined?this.props.values.dateFrom:null,
            dateTo:this.props.values.dateTo!=undefined?this.props.values.dateTo:null,
            categories:this.props.values.categories!=undefined?this.props.values.categories:[],
            keyWord:this.props.values.keyWord!=undefined?this.props.values.keyWord:null
        })
    };
    handleFilter = () => {
        this.setState({ open: false });
    }

    onClickCallBack = (coords) => {
        this.setState({ selectedPos: [coords.lat, coords.lng] });

    }

    render() {
        return (
            <div>
                <Button variant="outlined" color="primary" onClick={this.handleClickOpen}>
                    Filter by location
            </Button>

                <Dialog fullWidth={true} open={this.state.open} onClose={this.handleClose} aria-labelledby="form-dialog-title">
                    <DialogTitle id="form-dialog-title">Filter by location</DialogTitle>
                    <DialogContent>
                        {this.props.values && this.props.values.radius &&
                            <div>
                                <div class="slidecontainer">
                                    <label>Radius is {this.props.values.radius} km</label>
                                    <Field name="radius" component="input"
                                        type="range"
                                        min="0" max="10000" value={this.props.values.radius}
                                        step="20"
                                        className="radius-slider"

                                    />
                                </div>


                            </div>

                        }
                        <div>
                            {
                                this.props.values &&
                                this.props.values.selectedPos != undefined &&
                                this.props.values.selectedPos.lat != undefined &&
                                this.props.values.selectedPos.lng != undefined &&
                                <div>
                                    <p>Current position on the Map is:</p>
                                    <p>latitude: {this.props.values.selectedPos.lat}</p>
                                    <p>longitude: {this.props.values.selectedPos.lng}</p>
                                    {this.props.values && this.state.map == false && <DisplayMap location={{ latitude: this.props.values.selectedPos.lat, longitude: this.props.values.selectedPos.lng }} />}
                                </div>

                            }
                            {
                                this.props.values &&
                                this.props.values.selectedPos.lat == null &&
                                this.props.values.selectedPos.lng == null &&
                                <div>
                                    <p>Choose position on the Map!</p>
                                </div>
                            }

                            <Field
                                name='selectedPos'
                                component={LocationMap}
                                onClickCallBack={this.onClickCallBack}
                                circle
                                radius={this.props.values.radius}
                                initialValues={this.props.values}
                                events={this.props.events}
                            />

                        </div>
                    </DialogContent>
                    <DialogActions>
                    <Button onClick={this.handleClear} color="primary">
                            Clear
                    </Button>
                        <Button onClick={this.handleClose} color="primary">
                            Cancel
                    </Button>
                        <Button onClick={this.handleFilter} color="primary">
                            Filter
                </Button>
                    </DialogActions>
                </Dialog>
            </div>
        );
    }
}

export default MapModal;