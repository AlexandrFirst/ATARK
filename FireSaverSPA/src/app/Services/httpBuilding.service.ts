import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { observable, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { BuilderFilterSearch } from '../Models/BuildingService/builderFilterSearch';
import { BuildingInfoDto } from '../Models/BuildingService/buildingInfoDto';
import { NewBuildingDto } from '../Models/BuildingService/newBuildingDto';
import { UpdateBuildingDto } from '../Models/BuildingService/updateBuildingDto';
import { BaseHttpService } from './baseHttp.service';
import { } from "googlemaps"
import { ShelterDto } from '../Models/BuildingService/ShelterDto';
import { BuildingCenterDto } from '../Models/BuildingService/buildingCenterDto';
declare var google: any;

@Injectable({
  providedIn: 'root'
})
export class HttpBuildingService extends BaseHttpService {

  geocoder: google.maps.Geocoder;

  constructor(httpClient: HttpClient) {
    super(httpClient);
    this.geocoder = new google.maps.Geocoder();
  }

  getAllBuilding(searchParams: BuilderFilterSearch): Observable<HttpResponse<BuildingInfoDto[]>> {

    let params = new HttpParams();
    if (searchParams.pageNumber)
      params = params.append('PageNumber', searchParams.pageNumber.toString());

    if (searchParams.pageSize)
      params = params.append('PageSize', searchParams.pageSize.toString());

    if (searchParams.buildingId)
      params = params.append('BuildingId', searchParams.buildingId.toString());

    if (searchParams.address)
      params = params.append('Address', searchParams.address);

    return this.client.get<BuildingInfoDto[]>(this.baseUrl + "admin/allBuildingsInfo", { observe: 'response', params: params });
  }

  updateBuildingInfo(updatedBuildingInfo: UpdateBuildingDto): Observable<BuildingInfoDto> {
    return this.client.put<BuildingInfoDto>(this.baseUrl + "Building/updateBuilding/" + updatedBuildingInfo.id, {
      address: updatedBuildingInfo.address,
      info: updatedBuildingInfo.info,
      buildingCenterPosition: updatedBuildingInfo.buildingCenterPosition,
      region: updatedBuildingInfo.region
    })
  }

  addBuilding(newBuildingInfo: NewBuildingDto): Observable<BuildingInfoDto> {
    return this.client.post<BuildingInfoDto>(this.baseUrl + "Building/newbuilding", newBuildingInfo);
  }

  setBuildingCenter(buildingId: number, buildingCenter: BuildingCenterDto): Observable<any> {
    return this.client.put(this.baseUrl + `Building/setBuildingCenter/${buildingId}`, buildingCenter);
  }

  deleteBuilding(buildingId: number): Observable<any> {
    return this.client.delete(this.baseUrl + "Building/" + buildingId);
  }

  getBuildingById(buildingId: number): Observable<BuildingInfoDto> {
    return this.client.get<BuildingInfoDto>(this.baseUrl + "Building/info/" + buildingId);
  }

  setResponsibleUserForBuilding(buildingId: number, userMail: string): Observable<BuildingInfoDto> {
    return this.client.get<BuildingInfoDto>(this.baseUrl + `Building/adduser/${userMail}/${buildingId}`);
  }

  removeResponsibleUser(userId: number): Observable<BuildingInfoDto> {
    return this.client.delete<BuildingInfoDto>(this.baseUrl + `Building/removeuser/${userId}`);
  }

  getRegions(): Observable<String[]>{
    return this.client.get<String[]>(this.baseUrl + `Building/regions`);
  }

  validateBuildingAdress(address: string): Observable<any> {
    console.log('Building address: ', address)
    console.log("Google api called(geocoding)...");
    const observable = new Observable<any>(observer => {
      this.geocoder.geocode({ "address": address }, (results: google.maps.GeocoderResult[],
        status: google.maps.GeocoderStatus) => {
        console.log(results, status)
        if (status == 'OK') {
          observer.next(results[0].geometry)
        } else {
          observer.error('Geocode was not successful for the following reason: ' + status)
        }
      });
    });
    return observable;
  }

  addShelterToBuilding(buildingId: number, shelter: ShelterDto): Observable<any> {
    return this.client.post(this.baseUrl + `Building/shelter/${buildingId}`, shelter);
  }
  updateShelter(shelterId: number, shelter: ShelterDto): Observable<any> {
    return this.client.put(this.baseUrl + `Building/shelter/${shelterId}`, shelter);
  }
  deleteShelter(shelterId: number): Observable<any> {
    return this.client.delete(this.baseUrl + `Building/shelter/${shelterId}`);
  }
  getShelter(shelterId: number): Observable<any> {
    return this.client.get(this.baseUrl + `Building/shelter/${shelterId}`);
  }
}
