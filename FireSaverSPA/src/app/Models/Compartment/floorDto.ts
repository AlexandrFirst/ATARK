import { CompartmentDto } from "./compartmentDto";

export class FloorDto extends CompartmentDto {
    level: number;
    rooms: CompartmentDto[];
}