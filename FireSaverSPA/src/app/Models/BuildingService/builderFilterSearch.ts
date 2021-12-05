import { PaginationParams } from "../Shared/paginationParams";

export class BuilderFilterSearch extends PaginationParams {
    buildingId: number;
    address: string;
}