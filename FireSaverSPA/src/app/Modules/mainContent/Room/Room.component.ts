import { Component, OnInit } from '@angular/core';
import { CompartmentDto } from 'src/app/Models/Compartment/compartmentDto';
import { HttpRoomService } from 'src/app/Services/httpRoom.service';

import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { HttpEvacuationPlanService } from 'src/app/Services/httpEvacuationPlan.service';
import { MatDialog } from '@angular/material/dialog';
import { HttpPointService } from 'src/app/Services/httpPoint.service';
import { HttpTestService } from 'src/app/Services/httpTest.service';

import { HttpIotService } from 'src/app/Services/httpIot.service';
import * as $ from 'jquery';
import { BaseCompartmentComponent, InitCallback } from '../BaseCompartment/BaseCompartment.component';
import { CompartmentAddDialogComponent } from '../compartment-add-dialog/compartment-add-dialog.component';

@Component({
  selector: 'app-Room',
  templateUrl: '../Floor/Floor.component.html',
  styleUrls: ['./Room.component.scss']
})
export class RoomComponent extends BaseCompartmentComponent<CompartmentDto> {


  constructor(private roomService: HttpRoomService,
    location: Location,
    activatedRoute: ActivatedRoute,
    toastrService: ToastrService,
    evacuationService: HttpEvacuationPlanService,
    matDialog: MatDialog,
    pointService: HttpPointService,
    testService: HttpTestService,
    iotService: HttpIotService) {
    super(location, activatedRoute, toastrService, evacuationService, matDialog, pointService, testService, iotService)
  }

  isCompartmentFloor() {
    return false;
  }

  protected initCompartmentInfo(callback: InitCallback = null): void {
    this.roomService.getRoomInfo(this.compartmentId).subscribe(data => {
      this.compartmentInfo = data;
      if(callback)
        callback();
    }, error => {
      this.toastrService.error("Can't get room info")
    })
  }

  // protected initExpandableList() {
  //   console.log("Floor component expandabel count: ", $('.collapse').length)
  //   $('.r').each((index, value) => {
  //     value.addEventListener('click', (e) => {
  //       $('.collapse').each((index1, value1) => {

  //         if (index == index1) {
  //           if (value1.classList.contains('show')) {
  //             return;
  //           } else {
  //             value1.classList.add('show')
  //           }
  //         }
  //         else {
  //           value1.classList.remove('show')
  //         }
  //       });
  //     })
  //   })
  // }

  canChangeCompartment(): boolean {
    return true;
  }

  updateCOmpartmentInfo() {
    const dialogRef = this.matDialog.open(CompartmentAddDialogComponent, {
      data: { roomInfo: this.compartmentInfo }
    });

    dialogRef.afterClosed().subscribe(data => {
      if (data) {
        this.roomService.updateRoomInfo(this.compartmentId, data).subscribe(response => {
          this.initCompartmentInfo();
          this.toastrService.success(`Room with id: ${this.compartmentId} updated`)
        }, error => {
          this.toastrService.error("SOmething went wrong! Try again")
        });
      }
    })
  }

}
