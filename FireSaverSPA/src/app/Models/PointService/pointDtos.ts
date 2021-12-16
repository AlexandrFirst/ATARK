export enum RoutePointType
{
    EXIT,
    ADDITIONAL_EXIT,
    BUILDING_EXIT,
    UPSTAIRS_LEFT,
    ADDITIONAL_PATH,
    UPSTAIRS_RIGHT,
    DOWNSTAIRS_LEFT,
    DOWNSTAIRS_RIGHT,
    MAIN_PATH,
    GATHERING_POINT,
    POMPIER,
    EXTINGUISHER,
    FIRE_ALARM,
    EMERGENCY_CALL,
    HYDRANT
}


export class Postion {
    longtitude: number;
    latitude: number;
}

export class ScalePointDto {
    id: number;
    mapPosition: Postion;
    worldPosition: Postion;
}

export class RoutePoint {
    id: number;
    mapPosition: Postion;
    childrenPoints: RoutePoint[];
    parentPoint: RoutePoint;
    routePointType: RoutePointType = RoutePointType.MAIN_PATH;
}

export class InputRoutePoint {
    parentRoutePointId: number;
    pointPostion: Postion;
}

export class DeletePointOutput {
    point1Id: number;
    point2Id: number;
}
