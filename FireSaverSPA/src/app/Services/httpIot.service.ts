import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IoTNewPostion } from '../Models/IoTService/ioTNewPostion';
import { Postion } from '../Models/PointService/pointDtos';
import { BaseHttpService } from './baseHttp.service';

@Injectable({
  providedIn: 'root'
})
export class HttpIotService extends BaseHttpService {

  constructor(client: HttpClient) {
    super(client);
  }

  addIotToDb(iotId: string): Observable<any> {
    return this.client.post(this.baseUrl + `IoT/newIot`, { iotIdentifier: iotId })
  }

  addIotToCompartment(compartmentId: number, iotId: string): Observable<any> {
    return this.client.post(this.baseUrl + `IoT/newIotToCompartment`, { compartmentId: compartmentId, ioTIdentifier: iotId })
  }

  removeIotFromCompartment(compartmentId: number, iotId: string): Observable<any> {
    return this.client.delete(this.baseUrl + `IoT/removeIoTFromCompartment/${iotId}/${compartmentId}`)
  }

  updateIotPos(iotId: string, newPos: Postion): Observable<IoTNewPostion> {
    return this.client.put<IoTNewPostion>(this.baseUrl + `IoT/updateIotPos/${iotId}`, newPos);
  }

}
