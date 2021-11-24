import { AfterViewInit, Component, OnInit, ViewChild, ViewChildren } from '@angular/core';
import { JsonPipe, Location } from '@angular/common';
import * as L from 'leaflet'
import * as $ from 'jquery';
import { LatLngBoundsLiteral } from 'leaflet';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { FloorDto } from 'src/app/Models/Compartment/floorDto';
import { ToastrService } from 'ngx-toastr';
import { HttpFloorService } from 'src/app/Services/httpFloor.service';
import { HttpEvacuationPlanService } from 'src/app/Services/httpEvacuationPlan.service';
import { HttpEventType } from '@angular/common/http';
import { EvacuationPlanDto } from 'src/app/Models/EvacuationPlan/evacuationPlanDto';
import { InputRoutePoint, Postion as Position, Postion, RoutePoint, ScalePointDto } from 'src/app/Models/PointService/pointDtos';
import { MatDialog } from '@angular/material/dialog';
import { PositionInputDialogComponent } from '../position-input-dialog/position-input-dialog.component';
import { HttpPointService } from 'src/app/Services/httpPoint.service';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';
import { CompartmentDto } from 'src/app/Models/Compartment/compartmentDto';
import { QrCodeDialogComponent } from '../qr-code-dialog/qr-code-dialog.component';
import { TestInput } from 'src/app/Models/TestModels/testInput';
import { TestDialogComponent } from '../test-dialog/test-dialog.component';

enum MapType {
  ScalePoints,
  RoutePoints
}

@Component({ template: '' })
export abstract class BaseCompartmentComponent<T extends CompartmentDto> implements OnInit, AfterViewInit {

  private mapType: MapType = MapType.ScalePoints;

  private map: any;
  private bounds: LatLngBoundsLiteral = [[0, 0], [1000, 1000]];

  protected compartmentId: number;

  compartmentInfo: T;
  evacPlanInfo: EvacuationPlanDto;
  testInfo: TestInput = null

  uploadingValue: number = 0;

  selectedMapPosition: Position;
  private currentMarker: L.Marker;

  private selectedPointOnMap: L.CircleMarker;

  private pointBaseColor: string = "#ff7800";
  private pointeSelectedColor: string = "#ff0000";

  scalePointMarkers = new Map();

  routePoints: RoutePoint[] = [];
  private routePointMarkers = new Map();
  private routePolylines = new Map();
  private selectedRoutePoint: RoutePoint;


  constructor(protected location: Location,
    protected activatedRoute: ActivatedRoute,
    protected toastrService: ToastrService,
    protected evacuationService: HttpEvacuationPlanService,
    protected matDialog: MatDialog,
    protected pointService: HttpPointService) { }

  ngAfterViewInit(): void {
  }

  centerMap() {
    if (this.map) {
      this.map.fitBounds(this.bounds);
    }
  }

  abstract isCompartmentFloor();

  ngOnInit() {
    this.initExpandableList();

    this.activatedRoute.params.subscribe((params: Params) => {
      this.compartmentId = params.Id

      if (this.compartmentId) {
        this.initCompartmentInfo();
        this.initEvacuationPlanInfo();

        if (this.compartmentInfo?.compartmentTest) {
          this.testInfo = this.compartmentInfo?.compartmentTest;
        }
      } else {
        this.toastrService.error("Unknown compartment id")
      }
    })
  }

  backBtnClick() {
    this.location.back();
  }

  uploadFile = (files) => {
    if (files.length === 0) {
      return;
    }
    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('evacPlanImgae', fileToUpload, fileToUpload.name);

    this.evacuationService.uploadEvacuationPlan(formData, this.compartmentId).subscribe(
      event => {
        if (event.type === HttpEventType.UploadProgress) {
          this.uploadingValue = Math.round(100 * event.loaded / event.total);
        }
        else if (event.type === HttpEventType.Response) {
          this.toastrService.success("Plan is uploaded")
          this.evacPlanInfo = event.body;
          this.uploadingValue = 0;
          this.initMap();
        }
      },
      error => {
        this.uploadingValue = 0;
        this.toastrService.error("Something went wrong! Try again")
      }
    )
  }

  displayRoutePoints() {
    this.mapType = MapType.RoutePoints;
    this.initMapPoints();
  }

  displayScalePoints() {
    this.mapType = MapType.ScalePoints;
    this.initMapPoints();
  }


  protected abstract initCompartmentInfo(): void;

  private initExpandableList() {
    console.log("Floor component expandabel count: ", $('.collapse').length)
    $('.f').each((index, value) => {
      value.addEventListener('click', (e) => {
        $('.collapse').each((index1, value1) => {

          if (index == index1) {
            if (value1.classList.contains('show')) {
              return;
            } else {
              value1.classList.add('show')
            }
          }
          else {
            value1.classList.remove('show')
          }
        });
      })
    })
  }

  private initEvacuationPlanInfo() {
    if (this.compartmentId) {
      this.evacuationService.getEvacuationPlanOfCompartment(this.compartmentId).subscribe(response => {
        this.evacPlanInfo = response;
        this.initMap();
        this.initMapPoints();
      }, error => {
        this.toastrService.error("Can't get evacuation plan info")
      })
    }
  }



  private initMap() {
    if (this.evacPlanInfo) {
      this.map = L.map('myMap', {
        crs: L.CRS.Simple,
        maxZoom: 5,
        minZoom: -2,
      });
      var image = L.imageOverlay(this.evacPlanInfo.url, this.bounds).addTo(this.map);

      setTimeout(() => {
        this.centerMap()
      }, 1000)

      var icon = L.icon({
        iconUrl: 'assets/images/marker-icon.png',
        shadowUrl: 'assets/images/marker-shadow.png',
        iconAnchor: [10, 40],
      });

      this.map.on('click', (e) => {
        const pos = e.latlng;

        this.selectedMapPosition = {
          latitude: pos.lat,
          longtitude: pos.lng
        }

        if (this.currentMarker) {
          this.currentMarker.setLatLng(e.latlng);
        } else {
          this.currentMarker = L.marker(e.latlng, { icon }).addTo(this.map);
        }
      })
    }
  }

  private initMapPoints() {
    if (this.mapType == MapType.ScalePoints) {
      this.clearRoute()
      this.initScalePointMarkers();
    } else if (this.mapType == MapType.RoutePoints) {
      this.clearScalePoints();
      this.initRoutePointMarkers();
    }
  }

  initScalePointMarkers() {
    const currentScalePoints = this.evacPlanInfo?.scaleModel?.scalePoints;
    this.clearScalePoints()
    if (currentScalePoints) {
      currentScalePoints.forEach(scalePoint => {
        this.scalePointMarkers.set(scalePoint.id, this.placeMarker(scalePoint.mapPosition.latitude,
          scalePoint.mapPosition.longtitude,
          this.pointBaseColor));
      });
    }
  }

  clearScalePoints() {
    if (this.scalePointMarkers) {
      this.scalePointMarkers.forEach((value, key, map) => {
        var pointMarker = value;
        this.map.removeLayer(pointMarker);
      });
    }
  }

  addScalePoint() {
    let dialogRef = this.matDialog.open(PositionInputDialogComponent);
    dialogRef.afterClosed().subscribe(data => {
      const sendData: ScalePointDto = {
        id: 0,
        mapPosition: this.selectedMapPosition,
        worldPosition: data
      };

      this.pointService.addScalePoint(sendData, this.evacPlanInfo.id).subscribe(response => {

        const newScalePointMarker = this.placeMarker(this.selectedMapPosition.latitude,
          this.selectedMapPosition.longtitude,
          this.pointBaseColor)
        this.scalePointMarkers.set(response.id, newScalePointMarker)
      })
    })
  }


  removeScalePointMarker(pointId: number) {

    var dilaogRef = this.matDialog.open(ConfirmDialogComponent, {
      data: { message: `Are you sure you want to delete scale point with id: ${pointId}` }
    })

    dilaogRef.afterClosed().subscribe(data => {
      if (data == true) {
        this.pointService.removeScalePoint(pointId).subscribe(success => {
          const marker = this.scalePointMarkers.get(pointId);

          if (marker == this.selectedPointOnMap) {
            this.selectedPointOnMap = null;
          }

          this.map.removeLayer(marker);
          this.toastrService.success("Scale point with id: " + pointId + " deleted");
          this.scalePointMarkers.delete(pointId);

        }, error => {

          if (error.error?.message) {
            this.toastrService.warning(error.error?.message);

            const marker = this.scalePointMarkers.get(pointId);

            if (marker == this.selectedPointOnMap) {
              this.selectedPointOnMap = null;
            }

            this.map.removeLayer(marker);
            this.scalePointMarkers.delete(pointId);

          } else {
            this.toastrService.error("Something went wrong. Try again");
          }

        })
      }
    })
  }

  selectScalePoint(scalePointId: number) {
    const newlySelectedPoint = this.scalePointMarkers.get(scalePointId);
    this.selectPoint(newlySelectedPoint);
  }

  selectPoint(marker: L.CircleMarker) {

    if (this.selectedPointOnMap) {
      this.selectedPointOnMap.setStyle({ fillColor: this.pointBaseColor })
      this.selectedPointOnMap = marker;
      this.selectedPointOnMap.setStyle({ fillColor: this.pointeSelectedColor })
    } else {
      this.selectedPointOnMap = marker;
      this.selectedPointOnMap.setStyle({ fillColor: this.pointeSelectedColor })
    }
  }

  placeMarker(lat, lng, color): L.CircleMarker {

    const newRoutePoint: L.CircleMarker = L.circleMarker([lat, lng], {
      radius: 8,
      fillColor: color,
      color: color,
      weight: 1,
      opacity: 1,
      fillOpacity: 0.8
    }).addTo(this.map);
    return newRoutePoint;
  }

  initRoutePointMarkers() {
    this.pointService.getEvacRoutePointsForCompartment(this.compartmentId).subscribe(response => {
      console.log(response)
      this.printRouteFromPoint(response);
    }, error => {
      console.log(error)
      if (error.error?.Message) {
        this.toastrService.warning(error.error?.Message);
      }
      else {
        this.toastrService.error("Something went wrong")
      }
    })
  }

  addRoutePoint() {
    if (this.selectedMapPosition) {
      if (this.routePoints.length > 0) {
        if (this.selectedRoutePoint) {
          this.pointService.addPointToRouteEvacuationPlan({
            parentRoutePointId: this.selectedRoutePoint.id,
            pointPostion: this.selectedMapPosition
          } as InputRoutePoint).subscribe(response => {
            this.initRoutePointMarkers();
          })
        }
        else {
          this.toastrService.warning("Select route point to add a new one")
        }
      } else {
        this.pointService.addRouteToEvacuationPlan({
          parentRoutePointId: null,
          pointPostion: this.selectedMapPosition
        } as InputRoutePoint, this.compartmentId).subscribe(response => {
          this.initEvacuationPlanInfo();
        })
      }
    }
  }


  printRouteFromPoint(point: RoutePoint) {
    this.clearRoute();
    this.printRoute(point);
    console.log(this.routePoints)
  }

  private printRoute(currentPoint: RoutePoint) {
    var currentMarker = this.placeMarker(currentPoint.mapPosition.latitude, currentPoint.mapPosition.longtitude, this.pointBaseColor);

    this.routePoints.push(currentPoint);
    this.routePointMarkers.set(currentPoint.id, currentMarker);

    this.addPointHandler(currentPoint)

    if (currentPoint.childrenPoints.length == 0) {
      return;
    }

    currentPoint.childrenPoints.forEach(elem => {
      elem.parentPoint = {
        id: currentPoint.id
      } as RoutePoint;

      this.printRoute(elem);

      var newPolyline = L.polyline([this.routePointMarkers.get(currentPoint.id).getLatLng(), this.routePointMarkers.get(elem.id).getLatLng()]).addTo(this.map);
      this.routePolylines.set(elem.id, newPolyline);
    });
  }

  removeRoutePoint(routePointId: number) {
    this.pointService.deleteRoutePoint(routePointId).subscribe(response => {
      this.initRoutePointMarkers();
    })
  }

  clearRoute() {
    this.routePoints.forEach(routePoint => {

      const marker = this.routePointMarkers.get(routePoint.id);
      const polyline = this.routePolylines.get(routePoint.id);

      if (marker)
        this.map.removeLayer(marker);

      if (polyline)
        this.map.removeLayer(polyline);
    });

    this.routePointMarkers.clear();
    this.routePolylines.clear();
    this.routePoints = [];

  }

  getCompartmentQrCode() {
    const dialogRef = this.matDialog.open(QrCodeDialogComponent, {
      data: { info: JSON.stringify({ compartmentId: this.compartmentId }) }
    })
  }

  selectRoutePoint(routePointId: number): L.CircleMarker {
    console.log(this.routePointMarkers)
    const newlySelectedPoint = this.routePointMarkers.get(routePointId);
    this.selectedRoutePoint = this.routePoints.find(elem => elem.id == routePointId);
    this.selectPoint(newlySelectedPoint);
    return newlySelectedPoint
  }

  private addPointHandler(point: RoutePoint, isStartPoint: boolean = false) {

    var isPointUnderTrack = false;

    const selectedRouteMarker = this.selectRoutePoint(point.id)

    selectedRouteMarker.on("mousedown", () => {

      this.pointService.getRoutePointById(point.id).subscribe(data => {
        //this.selectedRoutePoint = data
        this.selectRoutePoint(data.id)
      })

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

        const newPos: Postion = {
          latitude: selectedRouteMarker.getLatLng().lat,
          longtitude: selectedRouteMarker.getLatLng().lng
        }

        point.mapPosition = newPos;
        this.pointService.updateRoutePointPostion(point).subscribe(success => {
          console.log("position is updated: ", success)
        })
      }
    })


    const trackCursor = (evt) => {

      console.log("Selected route point: ", this.selectedRoutePoint)

      selectedRouteMarker.setLatLng(evt.latlng)
      console.log(this.selectedRoutePoint)

      var oldPolyline = this.routePolylines.get(this.selectedRoutePoint.id);

      if (this.selectedRoutePoint.parentPoint) {
        console.log("Updating id there is parent root")
        oldPolyline.setLatLngs([this.routePointMarkers.get(this.selectedRoutePoint.parentPoint.id).getLatLng(),
        selectedRouteMarker.getLatLng()]);
      }

      var childArray: any = this.selectedRoutePoint.childrenPoints;

      if (childArray && childArray.length > 0) {
        childArray.forEach(point => {
          console.log(this.routePolylines);
          var polyline = this.routePolylines.get(point.id);
          polyline.setLatLngs([this.routePointMarkers.get(this.selectedRoutePoint.id).getLatLng(),
          this.routePointMarkers.get(point.id).getLatLng()])
        })
      }

    }

    this.routePointMarkers.set(point.id, selectedRouteMarker);


    // if (!isStartPoint) {
    //   var newPolyline = L.polyline([this.routePointMarkers.get(this.selectedRoutePoint.id).getLatLng(), selectedRouteMarker.getLatLng()]).addTo(this.map);

    //   this.routePolylines.set(point.id, newPolyline);
    // }

    this.selectedRoutePoint = point

    return selectedRouteMarker;
  }

  addTest() {
    const dialogRef = this.matDialog.open(TestDialogComponent)
  }


}
