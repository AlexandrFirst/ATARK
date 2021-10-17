import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { InputRoutePoint, Point, Postion } from '../Models/Dtos';

@Injectable({
  providedIn: 'root'
})
export class HttpServiceService {

  private baseUrl = "http://localhost:5000/";

  constructor(private client: HttpClient) { }

  sendPointData(newPoint: Point): Observable<any> {
    return this.client.post(this.baseUrl + "Points/newpos", newPoint);
  }

  calculateModel(): Observable<any> {
    return this.client.get(this.baseUrl + "Points/calculatePositionModel");
  }

  calculateImagePosition(position: Postion): Observable<any> {
    return this.client.post(this.baseUrl + "Points/mapPos", position);
  }

  deleteAllPoints(): Observable<any> {
    return this.client.delete(this.baseUrl + "Points/points");
  }

  sendRouteNewPoint(routePoint: InputRoutePoint) {
    return this.client.post(this.baseUrl + "RouteBuilder/newroute", routePoint);
  }

  addRouteNewPoint(routePoint: InputRoutePoint) {
    return this.client.post(this.baseUrl + "RouteBuilder/newpoint", routePoint);
  }

  deleteRoute() {
    return this.client.delete(this.baseUrl + "RouteBuilder/route");
  }

  getRoutePointById(routePointId: number):Observable<any> {
    return this.client.get(this.baseUrl + "RouteBuilder/routepoint/" + routePointId);
  }
}
