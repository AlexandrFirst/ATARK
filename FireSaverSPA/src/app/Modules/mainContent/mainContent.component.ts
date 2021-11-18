import { Component, OnInit } from '@angular/core';
import { BaseHttpService } from 'src/app/Services/baseHttp.service';

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

  constructor(private httpBaseService: BaseHttpService) { }

  //TODO: add iot to db according to role

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

}
