import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, NavigationExtras, Params, Router } from '@angular/router';
import { Location } from '@angular/common';
import * as $ from 'jquery';
import { BuildingInfoDto } from 'src/app/Models/BuildingService/buildingInfoDto';
import { HttpBuildingService } from 'src/app/Services/httpBuilding.service';
import { Toast, ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';
import { BuildingDialogComponent } from '../building-dialog/building-dialog.component';
import { CompartmentAddDialogComponent } from '../compartment-add-dialog/compartment-add-dialog.component';
import { ResponsibleUserAddDialogComponent } from '../responsible-user-add-dialog/responsible-user-add-dialog.component';
import { ConfirmDialogComponent } from '../../../Components/ConfirmDialog/confirm-dialog.component';
import { HttpFloorService } from 'src/app/Services/httpFloor.service';
import { FloorDto } from 'src/app/Models/Compartment/floorDto';
import { UpdateBuildingDto } from 'src/app/Models/BuildingService/updateBuildingDto';
import { FloorAddDialogComponent } from '../floor-add-dialog/floor-add-dialog.component';
import { } from "googlemaps"
import { Observable, of } from 'rxjs';
import { ShelterDialogComponent } from '../shelter-dialog/shelter-dialog.component';
import { ShelterDto } from 'src/app/Models/BuildingService/ShelterDto';
import { Postion } from 'src/app/Models/PointService/pointDtos';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
declare var google: any;

@Component({
  selector: 'app-Building',
  templateUrl: './Building.component.html',
  styleUrls: ['./Building.component.scss']
})
export class BuildingComponent implements OnInit {

  private buildingId;
  public buildingInfo: BuildingInfoDto;
  buildingPos: Postion;

  get buildingShelters() {
    if (this.buildingInfo)
      return this.buildingInfo.shelters
    return []
  }


  @ViewChild('worldMap') mapElement: any;
  worldMap: google.maps.Map;
  marker: google.maps.Marker;
  geocoder: google.maps.Geocoder;
  responseDiv: HTMLDivElement;
  response: HTMLPreElement;

  buildingShelter = new Map<number, google.maps.Marker>();

  mapsLoaded: Observable<boolean>;

  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private location: Location,
    private buildingService: HttpBuildingService,
    private toastrService: ToastrService,
    private matDialog: MatDialog,
    private floorService: HttpFloorService,
  ) {


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
      console.log("Retrieved building info: ", buildingInfo);
      if (!this.buildingInfo.buildingCenterPosition) {
        this.validateBuildingAdress(this.buildingInfo.address).subscribe(result => {

          this.buildingPos = {
            latitude: result.location.lat(),
            longtitude: result.location.lng()
          } as Postion;


          this.buildingService.updateBuildingInfo({
            address: buildingInfo.address,
            info: buildingInfo.info,
            id: buildingInfo.id,
            buildingCenterPosition: this.buildingPos
          } as UpdateBuildingDto).subscribe(success => {
            console.log(success)
            this.toastrService.success("Building with id: " + success.id + " updated");
            this.initBuildingInfo();
          }, error => {
            console.log(error);
            this.toastrService.error("Something bad happened! Try again");
          })


          console.log("Building pos", this.buildingPos);
        }, error => {
          this.toastrService.error("incorrect building adress! Fix this please: ", error)
        });
      }
      else {
        this.buildingPos = buildingInfo.buildingCenterPosition
      }



      console.log(this.buildingInfo)
    }, error => {
      this.toastrService.error("Can't find buiding with id: " + this.buildingId)
    })
  }

  validateBuildingAdress(adress): Observable<any> {
    return this.buildingService.validateBuildingAdress(adress);
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
        id: this.buildingId,
      },
      panelClass: "dialog-container-custom"
    })

    dialogRef.afterClosed().subscribe(data => {
      if (data) {
        console.log(data)
        this.buildingService.updateBuildingInfo({
          address: data.address,
          info: data.info,
          id: data.id,
          buildingCenterPosition: {
            latitude: data.pos.lat(),
            longtitude: data.pos.lng()
          }
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
    const takenFloors = this.buildingInfo.floors.map(floor => floor.level);
    let NavigationExtars: NavigationExtras = {
      state: {
        takenFloors: takenFloors
      }
    }
    this.router.navigate(['main', 'floor', floorId], NavigationExtars);
  }


  showShelters() {
    console.log("construction google map")
    console.log(this.buildingPos)
    if (this.buildingPos) {
      this.initShelterMap();
    } else {
      this.toastrService.error("can't be use with bad address")
    }
  }

  addShelter() {
    console.log("Shelter is adding")
    let dialogRef = this.matDialog.open(ShelterDialogComponent);
    dialogRef.afterClosed().subscribe((result: ShelterDto) => {
      console.log("new shelter data:", result)
      this.buildingService.addShelterToBuilding(this.buildingId, result).subscribe(data => {
        this.toastrService.success("new shelter added with id: ", data.id);
        const marker = new google.maps.Marker({
          position: { lat: data.shelterPosition.latitude, lng: data.shelterPosition.longtitude },
          map: this.worldMap,
          title: "Current building",
        });
        this.buildingShelter.set(data.id, marker);
        this.buildingShelters.push(data);
      })
    })
  }

  updateShelter(shelterId: number) {
    this.buildingService.getShelter(shelterId).subscribe(data => {
      let dialogRef = this.matDialog.open(ShelterDialogComponent, { data: data });
      dialogRef.afterClosed().subscribe(output => {
        this.buildingService.updateShelter(shelterId, output).subscribe(res => {
          this.toastrService.success("updated shelter with id: ", res.id);
          this.addShelterMarker(data);
        })
      });
    });
  }

  selectMarker(pos: Postion) {
    this.worldMap.setCenter({ lat: pos.latitude, lng: pos.longtitude });
    this.worldMap.setZoom(20);
  }

  deleteShelter(shelterId: number) {
    let dialogRef = this.matDialog.open(ConfirmDialogComponent, {
      data: { message: `Are you sure you want to delete shelter with id: ${shelterId}` }
    });
    dialogRef.afterClosed().subscribe(data => {
      if (data == true) {
        this.buildingService.deleteShelter(shelterId).subscribe(data => {
          this.toastrService.success("shelter is deleted");
          this.buildingShelter.get(shelterId).setMap(null);
          this.buildingShelter.delete(shelterId)

          const elem = this.buildingShelters.filter(s => s.id == shelterId)[0];
          const index = this.buildingShelters.indexOf(elem);
          this.buildingInfo.shelters = this.buildingShelters.splice(index, 1);
        })
      }
    });
  }

  initShelterMap() {
    console.log("construction google map")
    //get by adress location

    const mapProperties = {
      center: { lat: this.buildingPos.latitude, lng: this.buildingPos.longtitude },
      zoom: 17,
      mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    this.worldMap = new google.maps.Map(this.mapElement.nativeElement, mapProperties);

    new google.maps.Marker({
      position: { lat: this.buildingPos.latitude, lng: this.buildingPos.longtitude },
      map: this.worldMap,
      title: "Current building",
    });


    if (this.buildingInfo.shelters) {
      this.buildingInfo.shelters.forEach((data: ShelterDto) => {
        this.addShelterMarker(data);
      });
    }
  }

  private addShelterMarker(data: ShelterDto) {

    if (!data.shelterPosition) {
      this.buildingService.validateBuildingAdress(data.address).subscribe(result => {
        const marker = new google.maps.Marker({
          position: result.location,
          map: this.worldMap,
          title: "Current building",
        });
        this.buildingShelter.set(data.id, marker);

      }, error => {
        console.log(error)
        this.toastrService.error("Can't get shelter pos with id: " + data.id);
      })
    }
    else {
      const marker = new google.maps.Marker({
        position: { lat: data.shelterPosition.latitude, lng: data.shelterPosition.longtitude },
        map: this.worldMap,
        title: "Current building",
      });
      this.buildingShelter.set(data.id, marker);
    }
  }

}
