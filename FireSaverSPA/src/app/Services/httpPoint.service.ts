import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { InputRoutePoint, RoutePoint, ScalePointDto } from '../Models/PointService/pointDtos';
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

  getEvacRoutePointsForCompartment(compartmentId: number): Observable<RoutePoint> {
    return this.client.get<RoutePoint>(this.baseUrl + `RouteBuilder/compartment/${compartmentId}`);
  }

  addRouteToEvacuationPlan(routePoint: InputRoutePoint, compartmentId: number): Observable<RoutePoint> {
    return this.client.post<RoutePoint>(this.baseUrl + `RouteBuilder/${compartmentId}/newroute`, routePoint);
  }

  addPointToRouteEvacuationPlan(routePoint: InputRoutePoint): Observable<RoutePoint> {
    return this.client.post<RoutePoint>(this.baseUrl + `RouteBuilder/newpoint`, routePoint);
  }

  getRoutePointById(routePointId: number): Observable<RoutePoint> {
    return this.client.get<RoutePoint>(this.baseUrl + `RouteBuilder/routepoint/${routePointId}`);
  }

  updateRoutePointPostion(updatingRoutePoint: RoutePoint): Observable<RoutePoint> {
    return this.client.put<RoutePoint>(this.baseUrl + `RouteBuilder/updateMapPos`, updatingRoutePoint);
  }

  deleteRoutePoint(routePointId: number): Observable<any> {
    return this.client.delete(this.baseUrl + `RouteBuilder/routepoint/${routePointId}`)
  }
}
