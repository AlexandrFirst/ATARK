import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AddIotDialogComponent } from 'src/app/Components/add-iot-dialog/add-iot-dialog.component';
import { ResponsibleUserAddDialogComponent } from 'src/app/Components/responsible-user-add-dialog/responsible-user-add-dialog.component';
import { BaseHttpService } from 'src/app/Services/baseHttp.service';
import { HttpBuildingService } from 'src/app/Services/httpBuilding.service';
import { HttpIotService } from 'src/app/Services/httpIot.service';

@Component({
  selector: 'app-mainContent',
  templateUrl: './mainContent.component.html',
  styleUrls: ['./mainContent.component.scss']
})
export class MainContentComponent implements OnInit {

  buildingLink: string = '/main'

  isAdmin: boolean = false;
  isAuthUser: boolean = false;
  isGuest: boolean = false;

  reponsibleBuilding: number = null;

  constructor(private httpBaseService: BaseHttpService,
    private iotService: HttpIotService,
    private buildingService: HttpBuildingService,
    private matDialog: MatDialog,
    private toastr: ToastrService,
    private router: Router) { }

  ngOnInit() {
    var authData = this.httpBaseService.readAuthResponse();
    if (authData.roles.indexOf('Admin') >= 0) {
      this.buildingLink = this.buildingLink.concat('/buildings');
      this.isAdmin = true;

    } else if (authData.roles.indexOf('AuthorizedUser') >= 0) {
      const respBuildId = authData.responsibleBuildingId;
      this.isAuthUser = true;
      if (respBuildId != -1) {
        this.reponsibleBuilding = respBuildId;
        this.buildingLink = this.buildingLink.concat('buildings/' + this.reponsibleBuilding);
      }
    } else {
      this.isGuest = true;
    }
  }

  addIotToDb() {
    const dialofRef = this.matDialog.open(AddIotDialogComponent)
    dialofRef.afterClosed().subscribe(data => {
      if (data) {
        this.iotService.addIotToDb(data.iotIdField).subscribe(success => {
          this.toastr.success("Iot is added to db");
        }, error => {
          this.toastr.success("Try again");
        })
      }
    })
  }

  changeToUkraine() {
    const currentUrl = this.router.url;
    window.location.href = 'https://localhost:4201' + currentUrl
  }

  changeToEnglish() {
    const currentUrl = this.router.url;
    window.location.href = 'https://localhost:4200' + currentUrl
  }

  addResponsibleUserToBuilding() {
    var authData = this.httpBaseService.readAuthResponse();
    const dialofRef = this.matDialog.open(ResponsibleUserAddDialogComponent)
    dialofRef.afterClosed().subscribe(data => {
      if (data) {
        this.buildingService.setResponsibleUserForBuilding(authData.responsibleBuildingId, data.mail).subscribe(success => {
          this.toastr.success("Responsible user added is added to db");
        }, error => {
          this.toastr.success("Try again");
        })
      }
    })
  }
}
