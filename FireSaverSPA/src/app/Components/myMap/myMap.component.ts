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

 

    var sol: any = L.latLng([145, 175.2]);
    var icon = L.icon({
      iconUrl: 'assets/images/marker-icon.png',
      shadowUrl: 'assets/images/marker-shadow.png',
      popupAnchor: [13, 0],
    });
    this.marker = L.marker(sol, { icon }).addTo(this.map);
    this.map.setView([70, 120], 1);
    
    L.marker(L.latLng([100, 100]), { icon }).addTo(this.map);
    L.marker(L.latLng([100, 130]), { icon }).addTo(this.map);
    L.marker(L.latLng([130, 100]), { icon }).addTo(this.map);

    sol = this.xy(175.2, 145.0);
    const mizar = this.xy(41.6, 130.1);
    const kruegerZ = this.xy(13.4, 56.5);
    const deneb = this.xy(218.7, 8.3);

    L.marker(sol, { icon }).addTo(this.map).bindPopup('Sol');
    L.marker(mizar, { icon }).addTo(this.map).bindPopup('Mizar');
    L.marker(kruegerZ, { icon }).addTo(this.map).bindPopup('Krueger-Z');
    L.marker(deneb, { icon }).addTo(this.map).bindPopup('Deneb');


    var travel = L.polyline([sol, deneb]).addTo(this.map);

    this.map.on('click', (e) => {
      //alert("You clicked the map at " + e.latlng);

      const pos = e.latlng;

      this.postion = {
        latitude: pos.lng,
        longtitude: pos.lat
      };

      this.clickedCoords.emit(this.postion);
      this.marker.setLatLng(e.latlng)
    })

    this.inputPos.subscribe((val: Postion) => {
      console.log("Calculated pos received", val)
      var pos: any = L.latLng([val.longtitude, val.latitude]);
      var newmarker = L.marker(pos, { icon }).addTo(this.map);
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
