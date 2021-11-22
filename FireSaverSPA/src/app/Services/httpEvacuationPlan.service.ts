import { HttpClient, HttpEvent } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { EvacuationPlanDto } from '../Models/EvacuationPlan/evacuationPlanDto';
import { BaseHttpService } from './baseHttp.service';

@Injectable({
  providedIn: 'root'
})
export class HttpEvacuationPlanService extends BaseHttpService {

  constructor(httpClient: HttpClient) {
    super(httpClient);
  }

  uploadEvacuationPlan(formData: FormData, compartmentId: number): Observable<HttpEvent<EvacuationPlanDto>> {
    return this.client.post<EvacuationPlanDto>(this.baseUrl + `EvacuationPlan/${compartmentId}/newEvacPlan`, formData,
      { reportProgress: true, observe: 'events' });
  }

  getEvacuationPlanOfCompartment(compartmentId: number): Observable<EvacuationPlanDto> {
    return this.client.get<EvacuationPlanDto>(this.baseUrl + `EvacuationPlan/${compartmentId}`);
  }

}
