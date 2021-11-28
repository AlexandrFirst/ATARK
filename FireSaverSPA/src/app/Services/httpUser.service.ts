import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { InputRoutePoint, ScalePointDto, Postion, RoutePoint } from '../Models/PointService/pointDtos';
import { ServerResponseMessage } from '../Models/Shared/serverResponseMessage';
import { LoginUserDto, ResponseLoginDto } from '../Models/UserService/loginUserDto';
import { RegistrationUserData } from '../Models/UserService/registrationUserData';
import { UserInfoDto } from '../Models/UserService/userInfoDto';
import { UserUniqueMailResponse } from '../Models/UserService/UserUniqueMailResponse';
import { BaseHttpService } from './baseHttp.service';

@Injectable({
    providedIn: 'root'
})
export class HttpUserService extends BaseHttpService {

    constructor(client: HttpClient) {
        super(client);
    }

    IsMailUniq(mail: string): Observable<UserUniqueMailResponse> {
        console.log('validating mail ', mail, ' is user service')
        return this.client.get(this.baseUrl + "user/isMailUnique/" + mail).pipe(map(data => data as UserUniqueMailResponse));
    }

    SendUserRegistrationData(registrationData: RegistrationUserData): Observable<UserInfoDto> {
        return this.client.post(this.baseUrl + "user/newuser", registrationData).pipe(map(data => data as UserInfoDto));
    }

    LoginUser(loginData: LoginUserDto): Observable<ResponseLoginDto> {
        return this.client.post(this.baseUrl + "user/auth", loginData).pipe(map(data => data as ResponseLoginDto));
    }

    CheckTokenValidity(token: string): Observable<ServerResponseMessage> {
        return this.client.get(this.baseUrl + 'user/tokenValid').pipe(map(data => data as ServerResponseMessage));
    }

    GetUserInfoById(userId: number): Observable<UserInfoDto> {
        return this.client.get(this.baseUrl + 'user/' + userId).pipe(map(data => data as UserInfoDto));
    }

    UpdateUserInfo(updatingInfo: UserInfoDto): Observable<UserInfoDto> {
        return this.client.put(this.baseUrl + 'user/updateInfo', updatingInfo).pipe(map(data => data as UserInfoDto));
    }

    CheckIfUserCanBeResponsible(mail: string): Observable<any> {
        return this.client.get(this.baseUrl + 'user/canUserBeResponsible/' + mail);
    }
  
}