import { UserInfoDto } from "../UserService/userInfoDto";

export enum MessageType { FIRE, PERSONAL_HELP, OTHER_HELP, IOT }

export class RecievedMessageDto {
    id: number;
    sendTime: Date;
    user: UserInfoDto;
    messageType: MessageType;
    placeDescription: string;
}