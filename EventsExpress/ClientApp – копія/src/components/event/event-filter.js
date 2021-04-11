import React, { Component } from 'react';
import { reduxForm, Field } from 'redux-form';
import Button from "@material-ui/core/Button";
import Radio from '@material-ui/core/Radio';
import {
    renderTextField,
    renderDatePicker,
    renderMultiselect,
    radioButton
} from '../helpers/helpers';
import eventHelper from '../helpers/eventHelper';
import MapModal from './map-modal';
import './event-filter.css';
import DisplayMap from '../event/map/display-map';


class EventFilter extends Component {
    constructor(props) {
        super(props);
        this.state = {
            viewMore: false,
            needInitializeValues: true,
        };
    }
    
    componentDidUpdate(prevProps) {
        const initialValues = this.props.initialFormValues;
        if (!eventHelper.compareObjects(initialValues, prevProps.initialFormValues)
            || this.state.needInitializeValues) {
            this.props.initialize({
                keyWord: initialValues.keyWord,
                dateFrom: initialValues.dateFrom,
                dateTo: initialValues.dateTo,
                categories: initialValues.categories,
                status: initialValues.status,
                radius:initialValues.radius,
                // x:initialValues.x,
                // y:initialValues.y,
                // selectedPos:{x:initialValues.x,y:initialValues.y},
                selectedPos:{lat:initialValues.x,lng:initialValues.y},
                
            });
            this.setState({
                ['needInitializeValues']: false
            });
        }
    }
    ResetData=()=>{
        this.props.onReset();
    }
    render() {
        console.log("PROPS",this.props.initialFormValues,this.state.location);
        const { all_categories, form_values, current_user,initialFormValues } = this.props;
        let values = form_values || {};
        return <>
            <div className="sidebar-filter" >
                <form onSubmit={this.props.handleSubmit} className="box">
                    <div className="form-group">
                        <Field
                            name='keyWord'
                            component={renderTextField}
                            type="input"
                            label="Keyword"
                        />
                    </div>
                    {this.state.viewMore &&
                        <>
                            <div className="form-group">
                                <div>From</div>
                                <Field
                                    name='dateFrom'
                                    component={renderDatePicker}
                                />
                            </div>
                            <div className="form-group">
                                <div>To</div>
                                <Field
                                    name='dateTo'
                                    defaultValue={values.dateFrom}
                                    minValue={values.dateFrom}
                                    component={renderDatePicker}
                                />
                            </div>
                            <div className="form-group">
                                <Field
                                    name="categories"
                                    component={renderMultiselect}
                                    data={all_categories.data}
                                    valueField={"id"}
                                    textField={"name"}
                                    className="form-control mt-2"
                                    placeholder='#hashtags'
                                />
                            </div>
                            {current_user.role === "Admin" && (
                                <div className="form-group">
                                    <Field name="status" component={radioButton}>
                                        <Radio value="true" label="All" />
                                        <Radio value="true" label="Active" />
                                        <Radio value="true" label="Blocked" />
                                    </Field>
                                </div>
                        )}
                       
                        <div>
                            <MapModal initialize={this.props.initialize} initialValues={this.props.initialFormValues} values={values} reset={this.props.onReset} />
                        </div>
                        
                        <div className="d-flex">
                            
                            {values.selectedPos&&
                            values.selectedPos.lat&&
                            <div>
                                {console.log(values)}
                                <p>
                                Radius:
                                {values.radius}
                                </p>
                                <p>
                                Location:
                                {values.selectedPos.lat}
                               
                                {values.selectedPos.lng}
                                </p> 
                                <DisplayMap location={{latitude:values.selectedPos.lat,longitude:values.selectedPos.lng}}/>
                            </div>
                           
                            }
                             
                        </div>
                        </>
                    }
                    <div>
                        <Button
                            key={this.state.viewMore}
                            fullWidth={true}
                            color="secondary"
                            onClick={() => {
                                this.setState({ viewMore: !this.state.viewMore });
                            }}
                        >
                            {this.state.viewMore ? 'less...' : 'more filters...'}
                        </Button>
                    </div>
                    <div className="d-flex">
                        <Button
                            fullWidth={true}
                            color="primary"                            
                            onClick={this.props.onReset}
                            disabled={this.props.submitting}
                        >
                            Reset
                        </Button>
                        {current_user.id &&
                            <Button
                                fullWidth={true}
                                color="primary"
                                onClick={this.props.onLoadUserDefaults}
                                disabled={this.props.submitting}
                            >
                                Favorite
                            </Button>
                        }
                        <Button
                            fullWidth={true}
                            type="submit"
                            color="primary"
                            disabled={this.props.submitting}
                            //disabled={this.props.pristine || this.props.submitting}
                        >
                            Search
                        </Button>
                    </div>
                </form>
            </div>
        </>
    }
}

EventFilter = reduxForm({
    form: 'event-filter-form',
})(EventFilter);

export default EventFilter;
