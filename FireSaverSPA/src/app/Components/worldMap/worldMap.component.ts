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
    var icon = L.icon({
      iconUrl: 'assets/images/marker-icon.png',
      shadowUrl: 'assets/images/marker-shadow.png',
      popupAnchor: [13, 0],
    });


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
      this.marker = L.marker([pos.lat, pos.lng], {icon}).addTo(this.map)

      this.map.flyTo([pos.lat, pos.lng], 17);
      L.marker([pos.lat + 0.001, pos.lng], { icon }).addTo(this.map)
      L.marker([pos.lat, pos.lng + 0.001], { icon }).addTo(this.map)
      L.marker([pos.lat, pos.lng], { icon }).addTo(this.map)
    });

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
