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
import { HttpTestService } from 'src/app/Services/httpTest.service';
import { FloorAddDialogComponent } from '../floor-add-dialog/floor-add-dialog.component';




@Component({
  selector: 'app-Floor',
  templateUrl: './Floor.component.html',
  styleUrls: ['./Floor.component.scss']
})
export class FloorComponent extends BaseCompartmentComponent<FloorDto> {


  private takenFloors: number[] = null;

  constructor(private floorService: HttpFloorService,
    private router: Router,
    private roomService: HttpRoomService,
    location: Location,
    activatedRoute: ActivatedRoute,
    toastrService: ToastrService,
    evacuationService: HttpEvacuationPlanService,
    matDialog: MatDialog,
    pointService: HttpPointService,
    testService: HttpTestService) {
    super(location, activatedRoute, toastrService, evacuationService, matDialog, pointService, testService)

    this.activatedRoute.queryParams.subscribe(params => {
      if (this.router.getCurrentNavigation().extras.state) {
        this.takenFloors = this.router.getCurrentNavigation().extras.state.takenFloors
        console.log(this.takenFloors)
      }
    })
  }

  protected initCompartmentInfo(): void {
    this.floorService.getFloorInfo(this.compartmentId).subscribe(data => {
      this.compartmentInfo = data;
      console.log(data)
    }, error => {
      this.toastrService.error("Something went wrong! Reload the page")
    })
  }

  canChangeCompartment(): boolean {
    if (this.takenFloors)
      return true;
    return false;
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

  updateCOmpartmentInfo() {
    const dialogRef = this.matDialog.open(FloorAddDialogComponent, {
      data: { floorInfo: this.compartmentInfo, takenFloors: this.takenFloors }
    })

    dialogRef.afterClosed().subscribe(data => {
      if (data) {
        this.floorService.changeFloorOfBuilding(data, this.compartmentId).subscribe(success => {
          this.initCompartmentInfo();
          this.toastrService.success("Floor info updated")
        }, error => {
          this.toastrService.error("Something went wrong! Try again")
        })
      }
    })
  }

  changeRoomInfo(compartmentId: number) {
    var roomInfo = this.compartmentInfo.rooms.find(r => r.id == compartmentId);
    if (roomInfo) {
      const dialogRef = this.matDialog.open(CompartmentAddDialogComponent, {
        data: { roomInfo: roomInfo }
      });

      dialogRef.afterClosed().subscribe(data => {
        if (data) {
          this.roomService.updateRoomInfo(compartmentId, data).subscribe(response => {
            this.initCompartmentInfo();
            this.toastrService.success(`Room with id: ${compartmentId} updated`)
          }, error => {
            this.toastrService.error("SOmething went wrong! Try again")
          });
        }
      })
    }
  }

  deleteRoomInfo(compartmentId: number) {
    const dialogRef = this.matDialog.open(ConfirmDialogComponent, {
      data: { message: `Are you sure you want to delete room with id: ${this.compartmentId}` }
    });

    dialogRef.afterClosed().subscribe(data => {
      if(data){
        this.roomService.deleteRoom(compartmentId).subscribe(success => {
          this.toastrService.success(`Room with id: ${compartmentId} deleted`);
          this.initCompartmentInfo();
        })
      }
    })
  }

}
