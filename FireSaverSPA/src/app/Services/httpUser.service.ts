import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { InputRoutePoint, Point, Postion, RoutePoint } from '../Models/Dtos';
import { UserUniqueMailResponse } from '../Models/UserService/UserUniqueMailResponse';

@Injectable({
    providedIn: 'root'
})
export class HttpUserService {
    readonly baseUrl = environment.apiUrl;

    constructor(private client: HttpClient) {}

    IsMailUniq(mail: string): Observable<UserUniqueMailResponse> {
        return this.client.get(this.baseUrl + "user/isMailUnique/" + mail).pipe(map(data => data as UserUniqueMailResponse));
    }
}