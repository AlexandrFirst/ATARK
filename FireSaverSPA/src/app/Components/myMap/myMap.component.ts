import { AfterViewInit, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import * as L from 'leaflet'
import { LatLngBoundsLiteral } from 'leaflet';
import { Subject } from 'rxjs';
import { Postion } from 'src/app/Models/Dtos';

@Component({
  selector: 'app-myMap',
  templateUrl: './myMap.component.html',
  styleUrls: ['./myMap.component.scss']
})
export class MyMapComponent implements OnInit, AfterViewInit {

  constructor() { }
  map: any;
  marker: L.Marker;
  
  @Input() inputPos: Subject<Postion>;
  
  markersArray: L.Marker[] = [];

  @Output() clickedCoords: EventEmitter<Postion> = new EventEmitter<Postion>();
  postion: Postion;
  ngAfterViewInit(): void {
    this.map = L.map('myMap', {
      crs: L.CRS.Simple,
      maxZoom: 5,
      minZoom: -1,
    });



    var bounds: LatLngBoundsLiteral = [[0, 0], [1000, 1000]];
    var image = L.imageOverlay('assets/images/myPlace1000.png', bounds).addTo(this.map);

    this.map.fitBounds(bounds);

    var icon = L.icon({
      iconUrl: 'assets/images/marker-icon.png',
      shadowUrl: 'assets/images/marker-shadow.png',
      popupAnchor: [13, 0],
    });
       
    this.map.on('click', (e) => {
      const pos = e.latlng;

      this.postion = {
        latitude: pos.lng,
        longtitude: pos.lat
      };

      this.clickedCoords.emit(this.postion);
      if(!this.marker)
        this.marker = L.marker(e.latlng, {icon}).addTo(this.map);
      else
        this.marker.setLatLng(e.latlng)
    })

    this.inputPos.subscribe((val: Postion) => {
      console.log("Calculated pos received", val)
      var newmarker = L.marker([val.longtitude, val.latitude], { icon }).addTo(this.map);
      this.markersArray.push(newmarker);
    })
  }

  ngOnInit() {

  }

  yx = L.latLng;

  xy = function (x, y) {
    if (L.Util.isArray(x)) {    // When doing xy([x, y]);
      return this.yx(x[1], x[0]);
    }
    return this.yx(y, x);  // When doing xy(x, y);
  };

  removePlacedMarkers(){
    this.markersArray.forEach(marker => {
      this.map.removeLayer(marker)
    });
  }

}
