import { FloorDto } from "../Compartment/floorDto";
import { Postion } from "../PointService/pointDtos";
import { UserInfoDto } from "../UserService/userInfoDto";
import { ShelterDto } from "./ShelterDto";

export class BuildingInfoDto {
    id: number;
    responsibleUsers: UserInfoDto[]
    floors: FloorDto[]
    shelters: ShelterDto[]
    address: string;
    info: string;
    buildingCenterPosition: Postion;
    region:String;
}