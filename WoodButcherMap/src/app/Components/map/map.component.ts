import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.scss']
})
export class MapComponent implements OnInit {

  zoom = 12;
  center: google.maps.LatLngLiteral;
  options: google.maps.MapOptions = {
    mapTypeId: 'hybrid',
    zoomControl: false,
    scrollwheel: false,
    disableDoubleClickZoom: true,
    maxZoom: 15,
    minZoom: 8
  };

  markers = [];

  constructor() { 
    this.addMarker();
    this.addMarker();
    this.addMarker();
  }

  ngOnInit(): void {
    navigator.geolocation.getCurrentPosition(pos => {
      this.center = {
        lat: pos.coords.latitude,
        lng: pos.coords.longitude
      }
    });
    console.log('Center:');
    console.log(this.center);
  }

  addMarker () {
    this.markers.push ({
      position: {
        lat: this.center.lat + ((Math.random() - 0.5) * 2) / 10,
        lng: this.center.lng + ((Math.random() - 0.5) * 2) / 10,
      },
      label: {
        color: 'red',
        text: 'Marker label ' + (this.markers.length + 1),
      },
      title: 'Marker title' + (this.markers.length + 1),
      options: { animatiom: google.maps.Animation.BOUNCE }
    });
  }

  zoomIn() {
    if (this.zoom < this.options.maxZoom) {
      this.zoom++;
    }
  }

  zoomOut() {
    if (this.zoom > this.options.minZoom) {
      this.zoom--;
    }
  }
}
