import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class BaseHttpService {

  constructor(public client: HttpClient) { }

  public WriteToken(token: string){
    localStorage.setItem('token', token);
  }

  public GetToken(): string{
    return localStorage.getItem('token');
  }

  public RemoveToken(){
    localStorage.removeItem('token');
  }

}
