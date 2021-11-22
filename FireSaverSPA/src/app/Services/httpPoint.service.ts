import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ScalePointDto } from '../Models/PointService/pointDtos';
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
    console.log("deleting scale point with id: " + scalePointId)
    return this.client.delete(this.baseUrl + `ScalePoints/points/singlePoint/${scalePointId}`);
  }
}
