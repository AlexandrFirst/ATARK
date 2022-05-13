import { Postion } from "../PointService/pointDtos";

export class ShelterDto {
    id: number;
    address: string;
    capacity: number;
    info: string;
    shelterPosition: Postion
}