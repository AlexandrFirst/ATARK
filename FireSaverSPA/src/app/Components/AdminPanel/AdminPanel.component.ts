import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AdminService } from 'src/app/Services/Admin.service';
import { LoaderSignServiceService } from 'src/app/Services/LoaderSignService.service';

@Component({
  selector: 'app-AdminPanel',
  templateUrl: './AdminPanel.component.html',
  styleUrls: ['./AdminPanel.component.scss']
})
export class AdminPanelComponent implements OnInit {

  constructor(private adminService: AdminService,
    private toastr: ToastrService,
    private loadingService: LoaderSignServiceService) { }

  ngOnInit() {
  }

  backUpDB() {
    this.loadingService.activate();
    this.adminService.backupDatabase().subscribe(success => {
      this.loadingService.deactivate();
      const message = $localize`Db is updated`
      this.toastr.success(message)
    }, error => {
      this.loadingService.deactivate();
    })
  }

  restoreDB() {
    this.loadingService.activate();
    this.adminService.restoreDatabase().subscribe(success => {
      this.loadingService.deactivate();
      const message = $localize`Db is restored`
      this.toastr.success(message)
    }, error => {
      this.loadingService.deactivate();
    })
  }

}
