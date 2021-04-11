
import React, { Component } from 'react';
import {
    Map,
    TileLayer,
    Marker,
    Popup,
    Circle 
} from 'react-leaflet';
import Search from 'react-leaflet-search';
import { matchPath } from 'react-router-dom';
import './map.css';
import DisplayMap from '../../event/map/display-map';

const mapStyles = {
    width: "100%",
    height: "100%"
};

class LocationMap extends Component {
    constructor(props) {
        super(props);
        console.log("LocationMap",this.props);
        let value=null;
        let start=[50.4547, 30.5238];
        
        if(this.props.initialValues!=undefined){
            if(this.props.initialValues.x!=undefined){
                value={lat:this.props.initialValues.x,lng:this.props.initialValues.y};
                start = [this.props.initialValues.x,this.props.initialValues.y];
              }
        }
        
        if(this.props.selectedPos!=undefined){
            if(this.props.selectedPos[0]!=undefined){
                value={lat:this.props.selectedPos[0],lng:this.props.selectedPos[1]};
                start = [this.props.selectedPos[0],this.props.selectedPos[1]];
            }
           
        }
        console.log(this.props,value);
        this.state = {
            startPosition: start,
            selectedPos: value,
        }
        this.map = React.createRef();
    }

    setSelectedPos = (latlng) => {
        this.setState({ selectedPos: latlng });
        this.props.input.onChange(latlng);
    }

    handleClick = (e) => {

        this.setSelectedPos(e.latlng);
        console.log(e.latlng)
        if (this.props.onClickCallBack != null) {
            this.props.onClickCallBack(e.latlng);
        }
    }

    handleSearch = (e) => {
        this.setSelectedPos(e.latLng);
    }
    setZoom =(radius)=>{
        if(radius<100){
            return 15;
        }
       else if(radius<=600&&radius>=100){
            return 6;
        }
       else if(radius>600&&radius<=800){
            return 5;
        }
        else if(radius>800&&radius<=2200){
            return 4;
        }
        else if(radius>2200&&radius<=10000){
            return 2;
        }
        else{
            return 0.001;
        }
        
    }
    

    render() {
        const center = this.props.initialData || this.state.startPosition;
        const marker = this.state.selectedPos ? this.state.selectedPos : this.props.initialData;
        const { error, touched, invalid } = this.props.meta;
        let { circle, radius } = this.props;
        let value = this.state.selectedPos;
        let start=[50.4547, 30.5238];
        if(this.props.selectedPos!=undefined){
            if(this.props.selectedPos[0]!=undefined){
                value={lat:this.props.selectedPos[0],lng:this.props.selectedPos[1]};
                start = [this.props.selectedPos[0],this.props.selectedPos[1]];
            }
           
        }
        const selectedPos = value;
       // console.log("this.props",this.props);
        // let zoom=this.setZoom(radius);
        // const scale=Math.pow(2,zoom);
        // zoom=Math.log(scale) / Math.LN2;




    
    //let zoom=10;
    // if(this.map.leafletElement!=undefined){
    //     console.log(this.map.leafletElement);
    //     if(this.map.leafletElement._zoom!=undefined){
    //         zoom=this.map.leafletElement._zoom;
    //         console.log(this.map.leafletElement._zoom);

    //         //lat = this.map.leafletElement._animateToCenter.lat;
            
    //     }
    //     else{
    //         console.log("NOT AND")
    //     }
    // }
    let zoom=8;
    let resZoom=this.map.leafletElement!=undefined?this.map.leafletElement._zoom:8;
    console.log(this.map,resZoom)
    // let lat = selectedPos!=null?selectedPos.lat:50.4547;
    // const metersPerPixel = 156543.03392 * Math.cos(lat * Math.PI / 180) / Math.pow(2, resZoom);
    //const metersPerPixel=256 * Math.pow(1.5, resZoom);
    if(radius>=100&&radius<1000){
        console.log("Math.floor(radius/100)",Math.floor(radius/100),radius);
        radius=Number(Math.floor(radius/100)*5*100)+Number(radius);
        console.log("К",radius);
    }
    else if(radius>=1000&&radius<5000){
        radius=Number(Math.floor(radius/100)*2*1000)+Number(radius);
    }
    else if(radius>5000){
        radius=Number(Math.floor(radius/100)*5*1000)+Number(radius);
    }
    const metersPerPixel = (1.0083 * Math.pow(radius/1,0.5716) * (resZoom/2));
    
    console.log("radiusRes",radius);
    //const radiusMap =resZoom==-8?0:metersPerPixel * (radius);
    //console.log("metersPerPixel",metersPerPixel,resZoom,radiusMap)
    
        return (
            <div
                className="mb-4"
                style={{ position: "relative", width: "100%", height: "40vh" }}
                id="my-map">
                <Map
                    ref={(ref) => { this.map = ref }}
                    id="map"
                    style={mapStyles}
                    center={start}                   
                    zoom={resZoom}
                    onClick={this.handleClick}>
                        
                    <TileLayer
                        url="https://{s}.basemaps.cartocdn.com/rastertiles/voyager/{z}/{x}/{y}.png"
                    />
                    <Search
                        position="topright"
                        inputPlaceholder="Enter location"
                        showMarker={true}
                        zoom={7}
                        closeResultsOnClick={false}
                        openSearchOnLoad={false}
                        onChange={this.handleSearch}
                    />
                    {selectedPos &&
                        <Marker position={selectedPos} 
                            draggable={true}>
                            <Popup position={selectedPos}> 
                                <pre>
                                    {JSON.stringify(selectedPos, null, 2)}
                                </pre>
                            </Popup>
                        </Marker>
                    }
                    {circle && radius && selectedPos &&
                        <Circle center={start} pathOptions={{ color: 'blue' }} radius={metersPerPixel*1000}/>
                    }
                </Map>
                <span className="error-text">
                    {touched && invalid && error}
                </span>
            </div>
        );
    }
}

export default LocationMap;
