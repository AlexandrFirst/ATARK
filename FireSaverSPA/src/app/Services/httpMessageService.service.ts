import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseHttpService } from './baseHttp.service';

@Injectable({
  providedIn: 'root'
})
export class HttpMessageServiceService extends BaseHttpService {

  constructor(client: HttpClient) {
    super(client);
  }

  deleteMessage(messageId: number): Observable<any> {
    return this.client.delete(this.baseUrl + `Message/DeleteMessage/${messageId}`);
  }
}
