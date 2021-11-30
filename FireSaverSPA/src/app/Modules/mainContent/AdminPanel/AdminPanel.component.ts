import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AdminService } from 'src/app/Services/admin.service';
import { LoaderSignServiceService } from 'src/app/Services/LoaderSignService.service';

@Component({
  selector: 'app-AdminPanel',
  templateUrl: './AdminPanel.component.html',
  styleUrls: ['./AdminPanel.component.scss']
})
export class AdminPanelComponent implements OnInit {

  restorationIds: string[] = []

  constructor(private adminService: AdminService,
    private toastr: ToastrService,
    private loadingService: LoaderSignServiceService) { }

  ngOnInit() {
    this.GetAllRestorations()
  }

  backUpDB() {
    this.loadingService.activate();
    this.adminService.backupDatabase().subscribe(success => {
      this.loadingService.deactivate();
      const message = $localize`Db is updated`
      this.toastr.success(message)

      this.GetAllRestorations();
    }, error => {
      this.loadingService.deactivate();
    })
  }

  private GetAllRestorations(){
    this.adminService.getAllRestorations().subscribe(response => {
      this.restorationIds = response;
    }, error => {
      this.toastr.warning($localize`No restorations found`);
    })
  }

}
