import { AfterViewInit, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import * as L from 'leaflet'
import { LatLngBoundsLiteral } from 'leaflet';
import { Subject } from 'rxjs';
import { InputRoutePoint, Postion, RoutePoint } from 'src/app/Models/Dtos';
import { HttpServiceService } from 'src/app/Services/httpService.service';

@Component({
  selector: 'app-myMap',
  templateUrl: './myMap.component.html',
  styleUrls: ['./myMap.component.scss']
})
export class MyMapComponent implements AfterViewInit {

  constructor(private http: HttpServiceService) { }
  map: any;
  marker: L.Marker;

  parentRoutePoint: RoutePoint;
  allRoutesPoints: RoutePoint[] = [];

  routePoints: { [id: string]: L.CircleMarker; } = {}
  routePolyline: { [id: string]: L.Polyline; } = {}

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
        latitude: pos.lat,
        longtitude: pos.lng
      };

      this.clickedCoords.emit(this.postion);
      if (!this.marker)
        this.marker = L.marker(e.latlng, { icon }).addTo(this.map);
      else
        this.marker.setLatLng(e.latlng)
    })

    this.inputPos.subscribe((val: Postion) => {
      console.log("Calculated pos received", val)
      var newmarker = L.marker([val.latitude, val.longtitude], { icon }).addTo(this.map);
      this.markersArray.push(newmarker);
    })
  }

  placeRoutePointBtnClick() {
    if (!this.postion)
      return;

    if (!this.parentRoutePoint) {
      this.http.sendRouteNewPoint({ pointPostion: this.postion } as InputRoutePoint).subscribe((res: RoutePoint) => {
        var newPlaceMarker = this.placeRoutePoint(res.pointPostion.latitude, res.pointPostion.longtitude);
        newPlaceMarker.on('click', (e) => {
          this.parentRoutePoint = res
        })
        this.routePoints[res.id.toString()] = newPlaceMarker;
        this.allRoutesPoints.push(res);
        this.parentRoutePoint = res

      }, error => {
        console.log(error);
      });
    }
    else {
      this.http.addRouteNewPoint({ pointPostion: this.postion, parentRoutePointId: this.parentRoutePoint.id } as InputRoutePoint).subscribe((res: RoutePoint) => {
        var newPlaceMarker = this.placeRoutePoint(res.pointPostion.latitude, res.pointPostion.longtitude);
        newPlaceMarker.on('click', (e) => {
          this.parentRoutePoint = res
        })
        this.routePoints[res.id.toString()] = newPlaceMarker;
        this.allRoutesPoints.push(res);

        var newPolyline = L.polyline([this.routePoints[this.parentRoutePoint.id].getLatLng(), newPlaceMarker.getLatLng()]).addTo(this.map);

        this.routePolyline
        this.parentRoutePoint = res

        this.routePolyline[res.id.toString()] = newPolyline;
      }, error => {
        console.log(error);
      })
    }
  }

  placeRoutePoint(lat, lng): L.CircleMarker {

    const newRoutePoint: L.CircleMarker = L.circleMarker([lat, lng], {
      radius: 8,
      fillColor: "#ff7800",
      color: "#000",
      weight: 1,
      opacity: 1,
      fillOpacity: 0.8
    }).addTo(this.map);

    return newRoutePoint;
  }


  removePlacedMarkers() {
    this.markersArray.forEach(marker => {
      this.map.removeLayer(marker)
    });
  }

  deleteRouteBtnClick() {
    this.http.deleteRoute().subscribe(() => {
      this.parentRoutePoint = null;

      this.allRoutesPoints.forEach(item => {
        const marker: L.CircleMarker = this.routePoints[item.id.toString()];
        const line: L.Polyline = this.routePolyline[item.id.toString()];

        if (marker)
          this.map.removeLayer(marker);
        if (line)
          this.map.removeLayer(line);
      });
      this.allRoutesPoints = [];
      this.routePolyline = {}
      this.routePoints = {}
    }, error => {
      console.log(error);
    })
  }

}
