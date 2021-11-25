import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CompartmentDto } from '../Models/Compartment/compartmentDto';
import { BaseHttpService } from './baseHttp.service';

@Injectable({
  providedIn: 'root'
})
export class HttpRoomService extends BaseHttpService {

  constructor(client: HttpClient) {
    super(client)
  }

  getRoomInfo(roomId: number): Observable<CompartmentDto> {
    return this.client.get<CompartmentDto>(this.baseUrl + `Room/${roomId}`)
  }

  addRoomInfo(floorId: number, roomInfo: CompartmentDto): Observable<CompartmentDto> {
    return this.client.post<CompartmentDto>(this.baseUrl + `Room/addRoomToFloor/${floorId}`, roomInfo);
  }

  updateRoomInfo(roomId: number, roomInfo: CompartmentDto): Observable<CompartmentDto> {
    return this.client.put<CompartmentDto>(this.baseUrl + `Room/changeRoomInfo/${roomId}`, roomInfo);
  }

  deleteRoom(roomId: number): Observable<any> {
    return this.client.delete(this.baseUrl + `Room/${roomId}`);
  }
}
