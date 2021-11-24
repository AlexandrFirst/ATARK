import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Location } from '@angular/common';
import * as $ from 'jquery';
import { BuildingInfoDto } from 'src/app/Models/BuildingService/buildingInfoDto';
import { HttpBuildingService } from 'src/app/Services/httpBuilding.service';
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';
import { BuildingDialogComponent } from '../building-dialog/building-dialog.component';
import { CompartmentAddDialogComponent } from '../compartment-add-dialog/compartment-add-dialog.component';
import { ResponsibleUserAddDialogComponent } from '../responsible-user-add-dialog/responsible-user-add-dialog.component';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';
import { HttpFloorService } from 'src/app/Services/httpFloor.service';
import { FloorDto } from 'src/app/Models/Compartment/floorDto';
import { UpdateBuildingDto } from 'src/app/Models/BuildingService/updateBuildingDto';
import { FloorAddDialogComponent } from '../floor-add-dialog/floor-add-dialog.component';

@Component({
  selector: 'app-Building',
  templateUrl: './Building.component.html',
  styleUrls: ['./Building.component.scss']
})
export class BuildingComponent implements OnInit {

  private buildingId;
  buildingInfo: BuildingInfoDto;


  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private location: Location,
    private buildingService: HttpBuildingService,
    private toastrService: ToastrService,
    private matDialog: MatDialog,
    private floorService: HttpFloorService) {
   
  }

  ngOnInit() {
    this.activatedRoute.params.subscribe((params: Params) => {
      this.buildingId = params.buildingId

      if (this.buildingId) {
        this.initBuildingInfo();
      } else {
        this.toastrService.error("Unknown building id")
      }
    })
    console.log("Expandable list click event added")
    this.initExpandableList();
  }

  initBuildingInfo() {
    this.buildingService.getBuildingById(this.buildingId).subscribe(buildingInfo => {
      this.buildingInfo = buildingInfo;
      console.log(this.buildingInfo)
    }, error => {
      this.toastrService.error("Can't find buiding with id: " + this.buildingId)
    })
  }

  private initExpandableList() {
    console.log("Building component expandabel count: ", $('.collapse').length)

    $('.b').each((index, value) => {
     
      value.addEventListener('click', (e) => {
        console.log("clicked on list elem")
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

  backBtnClick() {
    this.location.back();
  }

  addResponsibleUser() {
    let dialogRef = this.matDialog.open(ResponsibleUserAddDialogComponent)
    dialogRef.afterClosed().subscribe(data => {
      if (data) {
        const userMail = data.mail;
        console.log(userMail)
        this.buildingService.setResponsibleUserForBuilding(this.buildingId, userMail)
          .subscribe(response => {
            this.buildingInfo = response;
            this.toastrService.success("New Responsible user added")
          }, error => {
            this.toastrService.error("Something went wrong. Try again")
          })

        console.log(data);
      }
    })
  }

  removeResponsibleUser(userId: number) {
    console.log(userId);

    let dialogRef = this.matDialog.open(ConfirmDialogComponent, {
      data: { message: `Are you sure you want to delete responsible user with id: ${userId}` }
    });
    dialogRef.afterClosed().subscribe(data => {
      if (data == true) {
        this.buildingService.removeResponsibleUser(userId).subscribe(data => {
          this.buildingInfo = data;
          this.toastrService.success(`User with id: ${userId} removed from responsibles`)
        }, error => {
          if (error.error?.message) {
            this.toastrService.error(error.error?.message);
          } else {
            this.toastrService.error("Something went wrong. Try again");
          }
        })
      }
    })

  }

  addFloor() {
    const takenFloors = this.buildingInfo.floors.map(floor => floor.level);
    let dialogRef = this.matDialog.open(FloorAddDialogComponent, {
      data: { takenFloors: takenFloors }
    })

    dialogRef.afterClosed().subscribe(data => {
      if (data) {
        const dataToSend: FloorDto = data;

        this.floorService.addFloorToBuilding(dataToSend, this.buildingId).subscribe(success => {
          this.initBuildingInfo();
          this.toastrService.success(`Floor with id: ${success.id} added to current building`);
        }, error => {
          this.toastrService.error("Something went wrong. Try again")
        })
      }
    })
  }

  changeFloor(floorId: number) {
    var currentFloorDto: FloorDto = this.buildingInfo.floors.filter(floor => floor.id == floorId)[0];
    const takenFloors = this.buildingInfo.floors.map(floor => floor.level);
    let dialogRef = this.matDialog.open(CompartmentAddDialogComponent, {
      data: { takenFloors: takenFloors, floorInfo: currentFloorDto }
    })

    dialogRef.afterClosed().subscribe(data => {
      if (data) {
        const dataToSend: FloorDto = data;

        this.floorService.changeFloorOfBuilding(dataToSend, floorId).subscribe(success => {
          this.initBuildingInfo();
          this.toastrService.success(`Floor with id: ${success.id} updated`);
        }, error => {
          this.toastrService.error("Something went wrong. Try again")
        })
      }
    })
  }

  deleteFloor(floorId: number) {

    let dialogRef = this.matDialog.open(ConfirmDialogComponent, {
      data: { message: `Are you sure you want to delete floor with id: ${floorId}` }
    });
    dialogRef.afterClosed().subscribe(data => {
      if (data == true) {
        this.floorService.deleteFloorFromBuilding(floorId).subscribe(data => {
          this.initBuildingInfo();
          this.toastrService.success(`Floor with id: ${floorId} removed from building`)
        }, error => {
          if (error.error?.message) {
            this.toastrService.error(error.error?.message);
          } else {
            this.toastrService.error("Something went wrong. Try again");
          }
        })
      }
    })
  }

  editBuilding() {
    console.log("Edit compartment with id: ", this.buildingId)
    let dialogRef = this.matDialog.open(BuildingDialogComponent, {
      data: {
        address: this.buildingInfo.address,
        info: this.buildingInfo.info,
        id: this.buildingId
      },
      panelClass: "dialog-container-custom"
    })

    dialogRef.afterClosed().subscribe(data => {
      if (data) {
        console.log(data)
        this.buildingService.updateBuildingInfo({
          address: data.address,
          info: data.info,
          id: data.id
        } as UpdateBuildingDto).subscribe(success => {
          console.log(success)
          this.toastrService.success("Building with id: " + success.id + " updated");
          this.initBuildingInfo();
        }, error => {
          console.log(error);
          this.toastrService.error("Something bad happened! Try again");
        })
      }
    });
  }

  goToFloorPage(floorId: number) {
    this.router.navigate(['main', 'floor', floorId]);
  }

}
