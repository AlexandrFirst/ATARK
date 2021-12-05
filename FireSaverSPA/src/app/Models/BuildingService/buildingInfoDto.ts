import { FloorDto } from "../Compartment/floorDto";
import { UserInfoDto } from "../UserService/userInfoDto";

export class BuildingInfoDto {
    id: number;
    responsibleUsers: UserInfoDto[]
    floors: FloorDto[]
    address: string;
    info: string;
}