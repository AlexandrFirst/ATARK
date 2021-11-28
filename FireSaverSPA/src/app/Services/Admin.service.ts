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

  restoreDatabase(): Observable<any> {
    return this.client.get(this.baseUrl + 'Admin/restore');
  }

  backupDatabase(): Observable<any> {
    return this.client.get(this.baseUrl + 'Admin/backup');
  }

  checkAdminRights(): Observable<any> {
    return this.client.get(this.baseUrl + 'Admin/checkRights')
  }
}
