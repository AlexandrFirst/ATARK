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
import { Postion as Position, ScalePointDto } from 'src/app/Models/PointService/pointDtos';
import { MatDialog } from '@angular/material/dialog';
import { PositionInputDialogComponent } from '../position-input-dialog/position-input-dialog.component';
import { HttpPointService } from 'src/app/Services/httpPoint.service';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-Floor',
  templateUrl: './Floor.component.html',
  styleUrls: ['./Floor.component.scss']
})
export class FloorComponent implements OnInit, AfterViewInit {

  private map: any;
  private bounds: LatLngBoundsLiteral = [[0, 0], [1000, 1000]];

  private floorId: number;

  floorInfo: FloorDto;
  evacPlanInfo: EvacuationPlanDto;

  uploadingValue: number = 0;

  private selectedMapPosition: Position;
  private currentMarker: L.Marker;

  private selectedPointOnMap: L.CircleMarker;

  private pointBaseColor: string = "#ff7800";
  private pointeSelectedColor: string = "#ff0000";

  //private scalePointMarkers: { [id: number]: L.CircleMarker } = {};
  private scalePointMarkers = new Map();

  constructor(private location: Location,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private toastrService: ToastrService,
    private floorService: HttpFloorService,
    private evacuationService: HttpEvacuationPlanService,
    private matDialog: MatDialog,
    private pointService: HttpPointService) { }

  ngAfterViewInit(): void {
  }

  centerMap() {
    if (this.map) {
      this.map.fitBounds(this.bounds);
    }
  }

  ngOnInit() {

    this.initExpandableList();

    this.activatedRoute.params.subscribe((params: Params) => {
      this.floorId = params.floorId

      if (this.floorId) {
        this.initFloorInfo();
        this.initEvacuationPlanInfo();
      } else {
        this.toastrService.error("Unknown building id")
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

    this.evacuationService.uploadEvacuationPlan(formData, this.floorId).subscribe(
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


  private initFloorInfo() {
    this.floorService.getFloorInfo(this.floorId).subscribe(data => {
      this.floorInfo = data;
    }, error => {
      this.toastrService.error("Something went wrong! Reload the page")
    })
  }

  private initExpandableList() {
    console.log("Floor component expandabel count: ", $('.collapse').length)
    $('.card').each((index, value) => {
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
    if (this.floorId) {
      this.evacuationService.getEvacuationPlanOfCompartment(this.floorId).subscribe(response => {
        this.evacPlanInfo = response;
        this.initMap();
        this.initScalePointMarkers();
      }, error => {
        this.toastrService.error("Can't get evacuation plan info")
      })
    }
  }

  initScalePointMarkers() {
    const currentScalePoints = this.evacPlanInfo?.scaleModel?.scalePoints;
    if (currentScalePoints) {
      currentScalePoints.forEach(scalePoint => {
        this.scalePointMarkers.set(scalePoint.id, this.placeMarker(scalePoint.mapPosition.latitude,
          scalePoint.mapPosition.longtitude,
          this.pointBaseColor));
      });
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
        }, error => {
          this.toastrService.error("Something went wrong! Try again")
        })
      }
    })
  }

  selectPoint(pointId: number) {
    const newlySelectedPoint = this.scalePointMarkers.get(pointId);
    if (this.selectedPointOnMap) {
      this.selectedPointOnMap.setStyle({ fillColor: this.pointBaseColor })
      this.selectedPointOnMap = newlySelectedPoint;
      this.selectedPointOnMap.setStyle({ fillColor: this.pointeSelectedColor })
    } else {
      this.selectedPointOnMap = newlySelectedPoint;
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
}
