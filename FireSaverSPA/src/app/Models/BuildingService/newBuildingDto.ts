import { Postion } from "../PointService/pointDtos";

export class NewBuildingDto {
    address: string;
    info: string;
    buildingCenterPosition: Postion;
    region:String;
}