import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr'
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { DeleteMessageDto } from '../Models/MessageModels/DeleteMessageDto';
import { RecievedMessageDto } from '../Models/MessageModels/MessageRecievedDto';
import { BaseHttpService } from './baseHttp.service';

@Injectable({
  providedIn: 'root'
})
export class SignalRServiceService extends BaseHttpService {

  hubConnection: signalR.HubConnection;
  private isConnected: boolean = false;

  constructor(client: HttpClient) {
    super(client);
  }

  IsConnected(): boolean {
    return this.isConnected;
  }

  connectToHub(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(environment.apiUrl + "socket", {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,
        accessTokenFactory: () => localStorage.getItem('token')
      })
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log("Connected to the hub")
        this.isConnected = true;
      })
      .catch(err => {
        console.log("Error: " + err);
        this.isConnected = false;
      })
  }

  getAlarm(): Observable<boolean> {
    return new Observable<boolean>(observer => {
      this.hubConnection.on("Alarm", () => {
        console.log("alarm is on!!!");
        observer.next(true)
      })
    })
  }

  AlarmOff(): Observable<boolean> {
    return new Observable<boolean>(observer => {
      this.hubConnection.on("AlarmOff", () => {
        console.log("alarm is off!!!");
        observer.next(true)
      })
    })
  }

  MessageRecieved(): Observable<RecievedMessageDto>{
    return new Observable<RecievedMessageDto>(observer => {
      this.hubConnection.on("MessageRecieved", (message: RecievedMessageDto) => {
        console.log("Message about: ", message)
        observer.next(message);
      })
    });
  }

  MessageDelete(): Observable<DeleteMessageDto>{
    return new Observable<DeleteMessageDto>(observer => {
      this.hubConnection.on("DeleteMessage", (delMessageInfo: DeleteMessageDto) => {
        console.log("Message to delete: ", delMessageInfo)
        observer.next(delMessageInfo);
      })
    });
  }

  setAlaram(): Observable<any> {
    return this.client.get(this.baseUrl + 'user/alarm');
  }

  switchOffAlaram(): Observable<any> {
    return this.client.get(this.baseUrl + 'user/alarmoff')
  }
}
