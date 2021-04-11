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
import './map.css'

const mapStyles = {
    width: "100%",
    height: "100%"
};

class LocationMap extends Component {
    constructor(props) {
        super(props);
        this.state = {
            startPosition: [50.4547, 30.5238],
            selectedPos: null,
        }
        this.map = React.createRef();
    }

    setSelectedPos = (latlng) => {
        this.setState({ selectedPos: latlng });
        this.props.input.onChange(latlng);
    }

    handleClick = (e) => {
        this.setSelectedPos(e.latlng);
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
        const { selectedPos } = this.state;
        // const zoom=this.setZoom(radius);
        // const scale=Math.pow(2,zoom);
        // const lat =selectedPos!=null?selectedPos.lat:1;
        // const metersPerPixel = 156543.03392 * Math.cos(lat * Math.PI / 180) / Math.pow(2, zoom);
        // const radiusClick = metersPerPixel * radius;




    // const zoom =this.map.current!=null? this.map.current.leafletElement.getZoom():1
    // const lat =selectedPos!=null?selectedPos.lat:1;
    // //const lat = this.map.leafletElement.getCenter().lat;
    // const metersPerPixel = 156543.03392 * Math.cos(lat * Math.PI / 180) / Math.pow(2, zoom)
    // const radiusMap = metersPerPixel * radius;
    let zoom=10;
    // if(this.map.leafletElement!=undefined){
    //     if(this.map.leafletElement._animateToZoom!=undefined){
    //         zoom=this.map.leafletElement._animateToZoom;
    //         //lat = this.map.leafletElement._animateToCenter.lat;
            
    //     }
    // }
    
    let lat = selectedPos!=null?selectedPos.lat:1;
    const metersPerPixel = 156543.03392 * Math.cos(lat * Math.PI / 180) / Math.pow(2, zoom)
    const radiusMap = metersPerPixel * radius;
    
        return (
            <div
                className="mb-4"
                style={{ position: "relative", width: "100%", height: "40vh" }}
                id="my-map">
                <Map
                    ref={(ref) => { this.map = ref }}
                    id="map"
                    style={mapStyles}
                    center={center}
                    zoom={10}
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
                    {marker &&
                        <Marker position={marker} 
                            draggable={true}>
                            <Popup position={marker}> 
                                <pre>
                                    {JSON.stringify(marker, null, 2)}
                                </pre>
                            </Popup>
                        </Marker>
                    }
                    {circle && radius && selectedPos &&
                        <Circle center={selectedPos} pathOptions={{ color: 'blue' }} radius={radiusMap}/>
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
