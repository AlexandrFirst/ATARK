import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BuilderFilterSearch } from 'src/app/Models/BuildingService/builderFilterSearch';
import { BuildingInfoDto } from 'src/app/Models/BuildingService/buildingInfoDto';
import { NewBuildingDto } from 'src/app/Models/BuildingService/newBuildingDto';
import { UpdateBuildingDto } from 'src/app/Models/BuildingService/updateBuildingDto';
import { HttpBuildingService } from 'src/app/Services/httpBuilding.service';
import { BuildingDialogComponent } from '../building-dialog/building-dialog.component';
import { ConfirmDialogComponent } from '../../../Components/ConfirmDialog/confirm-dialog.component';

@Component({
  selector: 'app-Buildings',
  templateUrl: './Buildings.component.html',
  styleUrls: ['./Buildings.component.scss']
})
export class BuildingsComponent implements OnInit {


  private buildingSearchParams: BuilderFilterSearch = new BuilderFilterSearch();

  currentPage: number = 1;
  totalPages: number = 1;
  totalItems: number = 1;

  allBuildingInfo: BuildingInfoDto[] = [];

  constructor(private dialog: MatDialog,
    private buildingService: HttpBuildingService,
    private toastrService: ToastrService,
    private router: Router) { }

  ngOnInit() {
    this.initBuildingsInfo();
  }

  editBuilding($event) {
    console.log("Edit compartment with id: ", $event.id)
    let dialogRef = this.dialog.open(BuildingDialogComponent, {
      data: $event,
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
          this.initBuildingsInfo();
        }, error => {
          console.log(error);
          this.toastrService.error("Something bad happened! Try again");
        })
      }
    });
  }

  addNewBuilding() {
    let dialogRef = this.dialog.open(BuildingDialogComponent, {
      panelClass: "dialog-container-custom"
    })

    dialogRef.afterClosed().subscribe(data => {
      console.log(data);
      if (data) {
        this.buildingService.addBuilding({
          address: data.address,
          info: data.info
        } as NewBuildingDto).subscribe(success => {
          this.toastrService.success("New building with id: " + success.id + " added");
          this.initBuildingsInfo();
        })
      }
    })
  }

  private initBuildingsInfo() {
    this.buildingService.getAllBuilding(this.buildingSearchParams).subscribe(buildings => {
      console.log(buildings.body)
      console.log(buildings.headers.get("Pagination"))

      const paginationInfo = JSON.parse(buildings.headers.get("Pagination"));

      this.currentPage = paginationInfo.currentPage;
      this.totalPages = paginationInfo.totalPages;
      this.totalItems = paginationInfo.totalItems;

      this.buildingSearchParams.pageNumber = this.currentPage;

      this.allBuildingInfo = buildings.body;
    })
  }

  pageChanged($event: PageEvent) {
    this.buildingSearchParams.pageNumber = $event.pageIndex + 1;
    this.initBuildingsInfo();
  }

  deleteBuilding(buildingId: number) {
    let dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: { message: "Are you shure you want to delete building?" }
    });

    dialogRef.afterClosed().subscribe(data => {
      if (data == true) {
        this.buildingService.deleteBuilding(buildingId).subscribe(success => {
          this.toastrService.success("Building with id: " + buildingId + " deleted");
          this.initBuildingsInfo();
        }, error => {
          this.toastrService.success("Something went wrong! Try again");
        })
      }
    });

  }

  onSearchInput(value) {
    const inputRegex = new RegExp('^#(.*)$');
    var match = inputRegex.exec(value);
    if (match && match[1]) {
      this.buildingSearchParams.address = null;
      this.buildingSearchParams.buildingId = Number.parseInt(match[1]);
      console.log("The search is id: " + match[1])
    }
    else {
      this.buildingSearchParams.address = value;
      this.buildingSearchParams.buildingId = null;
      console.log("Search by address: " + value);
    }

    this.initBuildingsInfo();
  }

  goToBuildingPage(buildingId: number) {
    this.router.navigate(['main', 'buildings', `${buildingId}`]);
  }

}
