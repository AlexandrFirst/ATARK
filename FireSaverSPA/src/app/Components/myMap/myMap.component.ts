import { AfterViewInit, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import * as L from 'leaflet'
import { LatLngBoundsLiteral } from 'leaflet';
import { Subject } from 'rxjs';
import { DeletePointOutput, InputRoutePoint, Postion, RoutePoint } from 'src/app/Models/Dtos';
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

  selectedRouteMarker: L.CircleMarker = null;
  selectedRoutePoint: RoutePoint;
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

    if (!this.selectedRoutePoint) {
      this.http.sendRouteNewPoint({ pointPostion: this.postion } as InputRoutePoint).subscribe((res: RoutePoint) => {
        pointHandler(res, true);
      }, error => {
        console.log(error);
      });
    }
    else {
      this.http.addRouteNewPoint({ pointPostion: this.postion, parentRoutePointId: this.selectedRoutePoint.id } as InputRoutePoint).subscribe((res: RoutePoint) => {
        pointHandler(res, false);
      }, error => {
        console.log(error);
      })
    }


    const pointHandler = (res: RoutePoint, isStartPoint: boolean = false) => {

      var newPlaceMarker = this.placeRoutePoint(res.pointPostion.latitude, res.pointPostion.longtitude);
      var isPointUnderTrack = false;

      if (this.selectedRouteMarker) {
        this.selectedRouteMarker.setStyle({ fillColor: 'green' })
        this.selectedRouteMarker = newPlaceMarker;
        this.selectedRouteMarker.setStyle({ fillColor: 'red' })
      }
      else {
        this.selectedRouteMarker = newPlaceMarker;
        this.selectedRouteMarker.setStyle({ fillColor: 'red' })
      }

      newPlaceMarker.on("mousedown", () => {

        this.http.getRoutePointById(res.id).subscribe(data => {
          console.log(data)
          this.selectedRoutePoint = data

          if (this.selectedRouteMarker) {
            this.selectedRouteMarker.setStyle({ fillColor: 'green' })
            this.selectedRouteMarker = this.routePoints[data.id.toString()];
            this.selectedRouteMarker.setStyle({ fillColor: 'red' })
          }

        })


        console.log("point is tracked: ", isPointUnderTrack)
        if (!isPointUnderTrack) {
          this.map.dragging.disable()

          this.map.on("mousemove", trackCursor)
          isPointUnderTrack = true;
        }

      })

      this.map.on("click", () => {
        if (isPointUnderTrack) {
          this.map.dragging.enable()
          this.map.off("mousemove", trackCursor)
          isPointUnderTrack = false;
        }
      })


      const trackCursor = (evt) => {
        newPlaceMarker.setLatLng(evt.latlng)
        var oldPolyline = this.routePolyline[this.selectedRoutePoint.id.toString()];

        if (this.selectedRoutePoint.parentPoint) {
          oldPolyline.setLatLngs([this.routePoints[this.selectedRoutePoint.parentPoint.id].getLatLng(),
          newPlaceMarker.getLatLng()]);
        }

        var childArray: any = this.selectedRoutePoint.childrenPoints;

        if (childArray && childArray.length > 0) {
          childArray.forEach(point => {
            console.log("here");
            var polyline = this.routePolyline[point.id.toString()];
            polyline.setLatLngs([this.routePoints[this.selectedRoutePoint.id.toString()].getLatLng(),
            this.routePoints[point.id.toString()].getLatLng()])
          })
        }

      }

      this.routePoints[res.id.toString()] = newPlaceMarker;
      this.allRoutesPoints.push(res);


      if (!isStartPoint) {
        var newPolyline = L.polyline([this.routePoints[this.selectedRoutePoint.id].getLatLng(), newPlaceMarker.getLatLng()]).addTo(this.map);

        this.routePolyline[res.id.toString()] = newPolyline;
      }

      this.selectedRoutePoint = res

      return newPlaceMarker;
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
      this.selectedRoutePoint = null;
      console.log("route points: ", this.allRoutesPoints)
      console.log("route markers: ", this.routePoints)
      console.log("route lines: ", this.routePolyline)

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

  deleteSelectedRoutePoint() {
    if (!this.selectedRoutePoint) {
      console.log("Select point to delete")
      return;
    }

    var selectedRoutePointId = this.selectedRoutePoint.id;
    this.http.deleteRoutePoint(selectedRoutePointId).subscribe((data: DeletePointOutput) => {
      console.log("reterned data: ", data);
      console.log("selected point: ", selectedRoutePointId);

      const lineToDelete = this.routePolyline[selectedRoutePointId.toString()];
      const markerToDelete = this.routePoints[selectedRoutePointId.toString()];

      if (lineToDelete) {
        this.map.removeLayer(lineToDelete);
        delete this.routePolyline[selectedRoutePointId.toString()];
      }

      if (markerToDelete) {
        this.map.removeLayer(markerToDelete);
        delete this.routePoints[selectedRoutePointId.toString()];
      }

      if (data.point1Id && data.point2Id) {

        console.log(this.routePoints);



        var moreLineToDelete = this.routePolyline[data.point2Id.toString()]

        if (moreLineToDelete) {
          this.map.removeLayer(moreLineToDelete);
          delete this.routePolyline[data.point2Id.toString()]
        }

        if (!lineToDelete) {
          var moreLineToDelete2 = this.routePolyline[data.point1Id.toString()]

          if (moreLineToDelete2) {
            this.map.removeLayer(moreLineToDelete2);
            delete this.routePolyline[data.point1Id.toString()]
          }
        }

        var point1 = this.routePoints[data.point1Id.toString()];
        var point2 = this.routePoints[data.point2Id.toString()];

        var newPolyline = L.polyline([point1.getLatLng(), point2.getLatLng()]).addTo(this.map);


        if (!this.routePolyline[data.point2Id]) {
          this.routePolyline[data.point2Id] = newPolyline;
        } else if (!this.routePolyline[data.point1Id]) {
          this.routePolyline[data.point1Id] = newPolyline;
        } else {
          console.error("can't add polyline while deleting")
        }

        var routePoint = this.allRoutesPoints.filter(val => val.id == data.point1Id)[0];

        this.selectedRoutePoint = routePoint;

        if (this.selectedRouteMarker) {
          this.selectedRouteMarker.setStyle({ fillColor: 'green' })
          this.selectedRouteMarker = this.routePoints[this.selectedRoutePoint.id.toString()];
          this.selectedRouteMarker.setStyle({ fillColor: 'red' })
        }

      } else if (data.point1Id) {

        if (!lineToDelete) {
          var moreLineToDelete = this.routePolyline[data.point1Id.toString()]

          if (moreLineToDelete) {
            this.map.removeLayer(moreLineToDelete);
            delete this.routePolyline[data.point1Id.toString()]
          }
        }

        var routePoint = this.allRoutesPoints.filter(val => val.id == data.point1Id)[0];
        this.selectedRoutePoint = routePoint;


      } else if (!data.point1Id && !data.point2Id) {
        this.selectedRoutePoint = null;
        this.allRoutesPoints = [];
        this.routePolyline = {}
        this.routePoints = {}
      }

    }, error => {
      console.log(error);
    })
  }

}
