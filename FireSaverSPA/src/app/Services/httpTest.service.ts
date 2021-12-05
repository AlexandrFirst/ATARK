import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TestInput } from '../Models/TestModels/testInput';
import { BaseHttpService } from './baseHttp.service';

@Injectable({
  providedIn: 'root'
})
export class HttpTestService extends BaseHttpService {

  constructor(client: HttpClient) {
    super(client)
  }

  addTestToCompartment(compartmentId: number, testInfo: TestInput): Observable<TestInput> {
    return this.client.post<TestInput>(this.baseUrl + `Test/addCompartmentTest/${compartmentId}`, testInfo);
  }

  deleteTestFromCompartment(compartmentId: number): Observable<any> {
    return this.client.delete(this.baseUrl + `Test/removeCompartmentTest/${compartmentId}`);
  }

  updateCompartmentTest(testId: number, testInfo: TestInput): Observable<TestInput> {
    return this.client.put<TestInput>(this.baseUrl + `Test/updateCompartmentTest/${testId}`, testInfo)
  }
}
