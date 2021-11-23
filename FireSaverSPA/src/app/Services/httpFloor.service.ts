import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { FloorDto } from '../Models/Compartment/floorDto';
import { BaseHttpService } from './baseHttp.service';

@Injectable({
  providedIn: 'root'
})
export class HttpFloorService extends BaseHttpService {

  constructor(httpClient: HttpClient) {
    super(httpClient)
  }

  addFloorToBuilding(floorDto: FloorDto, buildingId: number): Observable<FloorDto> {
    return this.client.post<FloorDto>(this.baseUrl + `Floor/addFloorToBuilding/${buildingId}`, floorDto);
  }

  changeFloorOfBuilding(floorDto: FloorDto, floorId: number): Observable<FloorDto> {
    return this.client.put<FloorDto>(this.baseUrl + `Floor/changeFloorInfo/${floorId}`, floorDto);
  }

  deleteFloorFromBuilding(floorId: number): Observable<any> {
    return this.client.delete(this.baseUrl + `Floor/${floorId}`);
  }

  getFloorInfo(floorId: number): Observable<FloorDto> {
    return this.client.get<FloorDto>(this.baseUrl + `Floor/${floorId}`);
  }


}
