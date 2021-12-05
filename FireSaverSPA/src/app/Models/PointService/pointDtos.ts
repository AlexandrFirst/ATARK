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
}

export class InputRoutePoint {
    parentRoutePointId: number;
    pointPostion: Postion;
}

export class DeletePointOutput {
    point1Id: number;
    point2Id: number;
}
