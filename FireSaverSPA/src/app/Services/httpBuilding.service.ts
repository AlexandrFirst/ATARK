import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { BuilderFilterSearch } from '../Models/BuildingService/builderFilterSearch';
import { BuildingInfoDto } from '../Models/BuildingService/buildingInfoDto';
import { NewBuildingDto } from '../Models/BuildingService/newBuildingDto';
import { UpdateBuildingDto } from '../Models/BuildingService/updateBuildingDto';
import { BaseHttpService } from './baseHttp.service';

@Injectable({
  providedIn: 'root'
})
export class HttpBuildingService extends BaseHttpService {

  constructor(httpClient: HttpClient) {
    super(httpClient);
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
    return this.client.put<BuildingInfoDto>(this.baseUrl + "Building/updateBuilding/" + updatedBuildingInfo.id, { address: updatedBuildingInfo.address, info: updatedBuildingInfo.info })
  }

  addBuilding(newBuildingInfo: NewBuildingDto): Observable<BuildingInfoDto> {
    return this.client.post<BuildingInfoDto>(this.baseUrl + "Building/newbuilding", newBuildingInfo);
  }

  deleteBuilding(buildingId: number): Observable<any> {
    return this.client.delete(this.baseUrl + "Building/" + buildingId);
  }

}
