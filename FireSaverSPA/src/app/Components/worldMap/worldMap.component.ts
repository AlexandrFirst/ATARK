import { AfterViewInit, Component, EventEmitter, OnInit, Output } from '@angular/core';
import * as L from 'leaflet'
import { Postion } from 'src/app/Models/Dtos';

@Component({
  selector: 'app-worldMap',
  templateUrl: './worldMap.component.html',
  styleUrls: ['./worldMap.component.scss']
})
export class WorldMapComponent implements AfterViewInit {

  title = 'FireSaverSPA';
  map: any;
  marker: L.Marker;

  ngAfterViewInit(): void {
    this.loadMap();
  }

  @Output() clickedCoords: EventEmitter<Postion> = new EventEmitter<Postion>();
  postion: Postion;

  private loadMap(): void {
    this.map = L.map('map').setView([0, 0], 1);
    L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token={accessToken}', {
      attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
      maxZoom: 18,
      id: 'mapbox/streets-v11',
      tileSize: 512,
      zoomOffset: -1,
      accessToken: 'pk.eyJ1IjoiYWxleGFuZGVyMjI4IiwiYSI6ImNrdXBidXViYjI1d2IycW82anRtdzR2cWgifQ.1IIR1OToIUoQsYBvufgguA',
    }).addTo(this.map);

    new Promise((resolve, reject) => {

      navigator.geolocation.getCurrentPosition(resp => {

        resolve({ lng: resp.coords.longitude, lat: resp.coords.latitude });
      },
        err => {
          reject(err);
        });
    }).then((pos: any) => {
      console.log(pos);
      this.map.flyTo([pos.lat, pos.lng], 17);
      L.marker([pos.lat + 0.001, pos.lng]).addTo(this.map)
      L.marker([pos.lat, pos.lng + 0.001]).addTo(this.map)
      L.marker([pos.lat, pos.lng]).addTo(this.map)
    });


    var icon = L.icon({
      iconUrl: 'assets/images/marker-icon.png',
      shadowUrl: 'assets/images/marker-shadow.png',
      popupAnchor: [13, 0],
    });

    this.marker = L.marker([51.5, -0.09], { icon });
    this.marker.addTo(this.map);

    var circle = L.circle([51.508, -0.11], {
      color: 'red',
      fillColor: '#f03',
      fillOpacity: 0.5,
      radius: 500
    }).addTo(this.map);

    var polygon = L.polygon([
      [51.509, -0.08],
      [51.503, -0.06],
      [51.51, -0.047]
    ]).addTo(this.map);

    this.marker.bindPopup("<b>Hello world!</b><br>I am a popup.").openPopup();
    circle.bindPopup("I am a circle.");
    polygon.bindPopup("I am a polygon.");

    var popup = L.popup()
      .setLatLng([51.5, -0.09])
      .setContent("I am a standalone popup.")
      .openOn(this.map);


    this.map.on('click', (e) => {
      const pos = e.latlng;

      this.postion = {
        latitude: pos.lat,
        longtitude: pos.lng
      };

      this.marker.setLatLng(e.latlng);

      this.clickedCoords.emit(this.postion);
    });
  }

}
