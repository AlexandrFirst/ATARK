import { UserInfoDto } from "../UserService/userInfoDto";

export class BuildingInfoDto {
    id: number;
    responsibleUsers: UserInfoDto[]
    address: string;
    info: string;
}