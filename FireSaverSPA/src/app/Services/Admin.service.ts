import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseHttpService } from './baseHttp.service';

@Injectable({
  providedIn: 'root'
})
export class AdminService extends BaseHttpService {

  constructor(client: HttpClient) {
    super(client)
  }

  restoreDatabase(backupId: string): Observable<any> {
    return this.client.get(this.baseUrl + `Admin/restore/${backupId}`);
  }

  backupDatabase(): Observable<any> {
    return this.client.get(this.baseUrl + 'Admin/backup');
  }

  checkAdminRights(): Observable<any> {
    return this.client.get(this.baseUrl + 'Admin/checkRights')
  }

  getAllRestorations(): Observable<string[]> {
    return this.client.get<string[]>(this.baseUrl + 'Admin/allRestorations');
  }

  deleteRestoration(backupId: string): Observable<any> {
    return this.client.delete(this.baseUrl + `Admin/deleteRestoration/${backupId}`);
  }
}
