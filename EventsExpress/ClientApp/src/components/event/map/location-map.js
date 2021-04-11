import React, { Component } from 'react';
import {
    Map,
    TileLayer,
    Marker,
    Popup,
    Circle
} from 'react-leaflet';
import Search from 'react-leaflet-search';
import './map.css';

const mapStyles = {
    width: "100%",
    height: "100%"
};

class LocationMap extends Component {
    constructor(props) {
        super(props);
        console.log(this.props);
        const {value,start}=this.getStartAndValue();
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
        if (this.props.onClickCallBack != null) {
            this.props.onClickCallBack(e.latlng);
        }
    }

    handleSearch = (e) => {
        this.setSelectedPos(e.latLng);
    }

    getCurrentZoom() {
        let defaultZoom = 8;
        return this.map.leafletElement != undefined ? this.map.leafletElement._zoom : defaultZoom;
    }
    getRadiusScale(radius) {
        let resZoom = this.getCurrentZoom();
        if (radius >= 100 && radius < 1000) {
            radius = Number(Math.floor(radius / 100) * 5 * 100) + Number(radius);
        }
        else if (radius >= 1000 && radius < 5000) {
            radius = Number(Math.floor(radius / 100) * 2 * 1000) + Number(radius);
        }
        else if (radius > 5000) {
            radius = Number(Math.floor(radius / 100) * 5 * 1000) + Number(radius);
        }
        return (1.0083 * Math.pow(radius / 1, 0.5716) * (resZoom / 2)) * 1000;
    }
    getStartAndValue() {
        let value = null;
        let start = [50.4547, 30.5238];//start position by default

        if (this.props.initialValues&&this.props.initialValues.selectedPos != undefined) {
            if ((this.props.initialValues.selectedPos.lat != null)&&(this.props.initialValues.selectedPos.lng != null)) {
                value = { lat: this.props.initialValues.selectedPos.lat, lng: this.props.initialValues.selectedPos.lat };
                start = [this.props.initialValues.selectedPos.lat, this.props.initialValues.selectedPos.lng];
            }
        }
        return{
            "value":value,
            "start":start,
        };
    }
    render() {
        const { error, touched, invalid } = this.props.meta;
        let { circle, radius } = this.props;
        let {start} = this.getStartAndValue();
        let resZoom = this.getCurrentZoom();
        let scaleRadius = this.getRadiusScale(radius);
        const marker = this.state.selectedPos ? this.state.selectedPos : this.props.initialData;
        console.log(this.props);
        const flag={"lat":50.4547,"lng":30.5238};
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
                    {this.props.initialValues&&this.props.initialValues.selectedPos &&
                        <Marker position={this.props.initialValues.selectedPos}
                            draggable={true}>
                            <Popup position={this.props.initialValues.selectedPos}>
                                <pre>
                                    {JSON.stringify(this.props.initialValues.selectedPos, null, 2)}
                                </pre>
                            </Popup>
                        </Marker>
                    }
                    {!this.props.initialValues&&marker &&
                        <Marker position={marker} 
                            draggable={true}>
                            <Popup position={marker}> 
                                <pre>
                                    {JSON.stringify(marker, null, 2)}
                                </pre>
                            </Popup>
                        </Marker>
                    }
                    {this.props.events!=undefined&&this.props.events.map(item=>
                        <Marker position={{"lat":item.location.latitude,"lng":item.location.longitude}} 
                            draggable={true}>
                                <Popup position={{"lat":item.location.latitude,"lng":item.location.longitude}}>
                                    <pre>
                                        {JSON.stringify({"lat":item.location.latitude,"lng":item.location.longitude}, null, 2)}
                                    </pre>
                                </Popup>
                        </Marker>
                    )
                        
                    }
                    {circle && radius &&
                        <Circle center={start} pathOptions={{ color: 'blue' }} radius={scaleRadius} />
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
