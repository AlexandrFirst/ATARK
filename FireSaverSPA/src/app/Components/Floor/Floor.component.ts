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
import { BaseCompartmentComponent } from '../BaseCompartment/BaseCompartment.component';
import { HttpRoomService } from 'src/app/Services/httpRoom.service';
import { CompartmentAddDialogComponent } from '../compartment-add-dialog/compartment-add-dialog.component';
import { CompartmentDto } from 'src/app/Models/Compartment/compartmentDto';




@Component({
  selector: 'app-Floor',
  templateUrl: './Floor.component.html',
  styleUrls: ['./Floor.component.scss']
})
export class FloorComponent extends BaseCompartmentComponent<FloorDto> {


  constructor(private floorService: HttpFloorService,
    private router: Router,
    private roomService: HttpRoomService,
    location: Location,
    activatedRoute: ActivatedRoute,
    toastrService: ToastrService,
    evacuationService: HttpEvacuationPlanService,
    matDialog: MatDialog,
    pointService: HttpPointService) {
    super(location, activatedRoute, toastrService, evacuationService, matDialog, pointService)
  }

  protected initCompartmentInfo(): void {
    this.floorService.getFloorInfo(this.compartmentId).subscribe(data => {
      this.compartmentInfo = data;
    }, error => {
      this.toastrService.error("Something went wrong! Reload the page")
    })
  }

  get FloorRooms() {
    return this.compartmentInfo?.rooms;
  }

  isCompartmentFloor() {
    return true;
  }

  addRoomInfo() {
    var dialogRef = this.matDialog.open(CompartmentAddDialogComponent, {
      data: {}
    })
    dialogRef.afterClosed().subscribe(data => {
      if (data) {
        const dataToSend: CompartmentDto = data;
        this.roomService.addRoomInfo(this.compartmentId, dataToSend).subscribe(response => {
          this.initCompartmentInfo();
        })
      }
    })
  }

  viewRoomInfo(roomId: number) {
    this.router.navigate(['main', 'room', roomId])
  }
}
