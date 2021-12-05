import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AdminService } from 'src/app/Services/admin.service';
import { LoaderSignServiceService } from 'src/app/Services/LoaderSignService.service';

@Component({
  selector: 'app-backup-list',
  templateUrl: './backup-list.component.html',
  styleUrls: ['./backup-list.component.scss']
})
export class BackupListComponent implements OnInit {

  @Input() restorationIds: string[] = []

  constructor(private adminService: AdminService,
    private toastr: ToastrService,
    private loadingService: LoaderSignServiceService) { }

  ngOnInit() {
  }

  restoreDb(restorationId: string) {
    this.loadingService.activate();
    this.adminService.restoreDatabase(restorationId).subscribe(success => {
      this.loadingService.deactivate();
      const message = $localize`Db is restored`
      this.toastr.success(message)
    }, error => {
      this.loadingService.deactivate();
    })
  }

  deleteBackup(restorationId: string) {
    this.loadingService.activate();
    this.adminService.deleteRestoration(restorationId).subscribe(success => {
      this.loadingService.deactivate();
      const message = $localize`Db is deleted`
      this.toastr.success(message)

      var backupIndex = this.restorationIds.findIndex(el => el == restorationId);
      if (backupIndex > -1) {
        this.restorationIds.splice(backupIndex, 1);
      }
    }, error => {
      this.loadingService.deactivate();
    })
  }

}
