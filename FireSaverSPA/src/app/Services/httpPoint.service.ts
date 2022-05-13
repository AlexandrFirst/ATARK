import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ExitPoint, InputRoutePoint, Postion, RoutePoint, ScalePointDto } from '../Models/PointService/pointDtos';
import { BaseHttpService } from './baseHttp.service';

@Injectable({
  providedIn: 'root'
})
export class HttpPointService extends BaseHttpService {

  constructor(client: HttpClient) {
    super(client)
  }

  addScalePoint(scalePoint: ScalePointDto, evacPlanId: number): Observable<ScalePointDto> {
    return this.client.post<ScalePointDto>(this.baseUrl + `ScalePoints/newpos/${evacPlanId}`, scalePoint);
  }

  removeScalePoint(scalePointId: number): Observable<any> {
    return this.client.delete(this.baseUrl + `ScalePoints/points/singlePoint/${scalePointId}`);
  }

  getExitPoints(compartmentId: number): Observable<any> {
    return this.client.get(this.baseUrl + `RouteBuilder/getExitPoints/${compartmentId}`);
  }

  deleteExitPoint(pointId: number): Observable<any> {
    return this.client.delete(this.baseUrl + `RouteBuilder/removeExitPoint/${pointId}`);
  }

  addExitPoint(compartmentId: number, markerPosition: Postion) {
    return this.client.post(this.baseUrl + `RouteBuilder/addExitPoint/${compartmentId}`, markerPosition);
  }

  TransformUserWorldPostion(compartmentId: number, userWorldPostion: Postion): Observable<Postion> {
    return this.client.post<Postion>(this.baseUrl + `user/GetTransformedPostions/${compartmentId}`, userWorldPostion);
  }
}
