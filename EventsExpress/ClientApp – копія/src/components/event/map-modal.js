import React from 'react';
import Button from '@material-ui/core/Button';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogTitle from '@material-ui/core/DialogTitle';
import LocationMap from './map/location-map';
import { makeStyles } from "@material-ui/core/styles";
import { Field } from 'redux-form';
import Typography from "@material-ui/core/Typography";
import Slider from "@material-ui/core/Slider";
import { Component } from 'react';
import eventHelper from '../helpers/eventHelper';
import './slider.css';
import DisplayMap from '../event/map/display-map';
 class MapModal extends Component
{
    constructor(props) {
        super(props);
        console.log("CONSTRUCTOR");
        this.state = {
            open:false,
            selectedPos:null,
            radius:this.props.initialValues.radius!=undefined?this.props.initialValues.radius:0,   
            needInitializeValues: true,
            map:false
        }
        this.baseState = this.state;
        
    }
    // componentDidUpdate(prevProps) {
    //     console.log("componentDidUpdate(prevProps)");     
    //     const initialValues = this.props.initialFormValues;
    //     console.log("initialValues",initialValues);
    //     if (!eventHelper.compareObjects(initialValues, prevProps.initialFormValues)
    //         || this.state.needInitializeValues) {
    //         this.props.initialize({
    //             radius:initialValues.radius,
    //             selectedPos:{initialValues.x,
    //             y:initialValues.y,                
    //         });
    //         this.setState({
    //             ['needInitializeValues']: false
    //                     });
    //                 }
    // }

    
    
    handleClickOpen= ()=>{
        this.setState({open:true});
    };
     handleClose = () => {
        console.log("handleClose");
        console.log("PROPS",this.props);
        let x=this.props.initialValues.x;
        let y=this.props.initialValues.y;
        this.setState({selectedPos:[x,y]});
        let radius = this.props.initialValues.radius;
        this.setState({radius:radius});
    };
    handleFilter = () =>{ 
        console.log("handleFilter");     
        this.setState({open:false});
    }

     onClickCallBack = (coords) => { 
        console.log("onClickCallBack");           
        this.setState({selectedPos:[coords.lat, coords.lng]});
        // this.setState({map:true})
        //  this.setState({
        //     ['needInitializeValues']: true
        // });
       
    }

    onRadiusChange = (event) => { 
        const { value } = event.target
        this.setState({radius:value});
        
    }

     pos = () => {
        if (this.state.selectedPos != null) {
            return (
                <div>
                    <div>{this.state.selectedPos[0]}</div>
                    <div>{this.state.selectedPos[1]}</div>
                    <div>{this.state.radius}</div>
                </div>
            );
        }
    }
   
    render(){
        let currentLocation="Choose position on the Map!";
        let location  ={latitude:0,longitude:0};
        if (this.state.selectedPos != null) {
            currentLocation="Current position on the Map is:"+"latitude:"+ this.state.selectedPos[0]+"longitude:"+this.state.selectedPos[1];
            location={latitude:this.state.selectedPos[0],longitude:this.state.selectedPos[1]}
        }     
         console.log("RENDER");
    return (
        <div>
            <Button variant="outlined" color="primary" onClick={this.handleClickOpen}>
                Filter by location
            </Button>
           
            <Dialog fullWidth={true} open={this.state.open} onClose={this.handleClose} aria-labelledby="form-dialog-title">
                <DialogTitle id="form-dialog-title">Filter by location</DialogTitle>
                <DialogContent>
                    {/* <Field name="radius" component = "input"
                           type="text" placeholder="radius"
                           onChange={this.onRadiusChange}
                           />  */}
                           
                    {((this.props.initialValues.x&&this.props.initialValues.y)||(this.state.selectedPos))&&
                   
                        <div class="slidecontainer">
                             <label>Radius is {this.state.radius}</label>
                               <Field name="radius" component="input"
                                   type="range"
                                   min="0" max="10000" value={this.props.initialValues.radius}
                                   onChange={this.onRadiusChange}
                                   step="any"
                                   className="radius-slider"/>
                                  
                           </div> 
                     }
                    {
                       
                    <div>
                        {
                            //this.state.selectedPos&&this.state.needInitializeValues==false&&
                            this.state.selectedPos&&
                            this.state.selectedPos[0]!=undefined&&
                            this.state.selectedPos[1]!=undefined&&
                            <div>
                                <p>Current position on the Map is:</p>
                                <p>latitude:{this.state.selectedPos[0]}</p>
                                <p>longitude:{this.state.selectedPos[1]}</p>
                                {console.log("DisplayMap location={location}")}
                                {this.state.map==false&&<DisplayMap location={location}/>}
                            </div>
                            
                        }
                        
                        {
                            this.state.selectedPos&&
                            this.state.selectedPos[0]==undefined&&
                            this.state.selectedPos[1]==undefined&&
                            <div>
                                <p>Choose position on the Map!</p>
                            </div>
                        }
                        <Field
                        name='selectedPos'
                        component={LocationMap}
                        onClickCallBack={this.onClickCallBack}
                        circle
                        radius={this.state.radius}
                        selectedPos = {this.state.selectedPos}
                        initialValues = {this.props.initialValues}
                        initialize = {this.props.initialize}                       
                    />
                   
                    </div>
                   }
                    {this.pos()}
                    
                </DialogContent>
                <DialogActions>
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