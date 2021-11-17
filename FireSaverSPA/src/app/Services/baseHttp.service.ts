import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ResponseLoginDto } from '../Models/UserService/loginUserDto';
import { ItemStorageHandler } from './IItemStorageHandler';
import { StorageItemHandler } from './StorageItemHandler';

@Injectable({
  providedIn: 'root'
})
export class BaseHttpService {

  private readonly tokenStorage: ItemStorageHandler;
  private readonly userStorage: ItemStorageHandler;
  private readonly roleStorage: ItemStorageHandler;
  private readonly responsobleBuildingId: ItemStorageHandler;


  constructor(public client: HttpClient) {
    if (!this.tokenStorage)
      this.tokenStorage = new StorageItemHandler('token');
    if (!this.userStorage)
      this.userStorage = new StorageItemHandler('userId');

    if (!this.responsobleBuildingId)
      this.responsobleBuildingId = new StorageItemHandler('responsibleBuildingId');
    if (!this.roleStorage)
      this.roleStorage = new StorageItemHandler('roles');
  }


  public writeAuthResponse(responseDto: ResponseLoginDto) {
    this.tokenStorage.Write(responseDto.token);
    this.userStorage.Write(responseDto.userId.toString());
    this.roleStorage.Write(responseDto.roles.join(','));
    this.responsobleBuildingId.Write(responseDto.responsibleBuildingId);
  }

  public readAuthResponse(): ResponseLoginDto {
    return {
      responsibleBuildingId: Number(this.responsobleBuildingId.Read()),
      roles: this.roleStorage.Read().split(','),
      token: this.tokenStorage.Read().toString(),
      userId: Number(this.userStorage.Read())
    } as ResponseLoginDto
  }

  public deleteStorage(){
    this.tokenStorage.Delete();
    this.userStorage.Delete();
    this.roleStorage.Delete();
    this.responsobleBuildingId.Delete();

  }

}
