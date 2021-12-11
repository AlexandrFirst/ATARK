import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RecievedMessageDto } from '../Models/MessageModels/MessageRecievedDto';
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

  getAllMessages(): Observable<RecievedMessageDto[]>{
    return this.client.get<RecievedMessageDto[]>(this.baseUrl + `Message/allMessages`);
  }
}
