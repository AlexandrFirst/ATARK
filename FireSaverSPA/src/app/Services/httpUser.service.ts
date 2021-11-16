import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { InputRoutePoint, Point, Postion, RoutePoint } from '../Models/Dtos';
import { LoginUserDto, ResponseLoginDto } from '../Models/UserService/loginUserDto';
import { RegistrationUserData } from '../Models/UserService/registrationUserData';
import { UserInfoDto } from '../Models/UserService/userInfoDto';
import { UserUniqueMailResponse } from '../Models/UserService/UserUniqueMailResponse';
import { BaseHttpService } from './baseHttp.service';

@Injectable({
    providedIn: 'root'
})
export class HttpUserService extends BaseHttpService {
    readonly baseUrl = environment.apiUrl;

    constructor(client: HttpClient) {
        super(client);
     }

    IsMailUniq(mail: string): Observable<UserUniqueMailResponse> {
        return this.client.get(this.baseUrl + "user/isMailUnique/" + mail).pipe(map(data => data as UserUniqueMailResponse));
    }

    SendUserRegistrationData(registrationData: RegistrationUserData): Observable<UserInfoDto> {
        return this.client.post(this.baseUrl + "user/newuser", registrationData).pipe(map(data => data as UserInfoDto));
    }

    LoginUser(loginData: LoginUserDto): Observable<ResponseLoginDto> { 
        return this.client.post(this.baseUrl + "user/auth", loginData).pipe(map(data => data as ResponseLoginDto));
    }
}