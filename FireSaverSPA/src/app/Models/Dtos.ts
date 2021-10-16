export class Postion {
    longtitude: number;
    latitude: number;
}

export class Point {
    mapPosition: Postion;
    worldPosition: Postion;
}

export class RoutePoint {
    id: number;
    pointPostion: Postion;
    childrenPoints: RoutePoint[];
    parentPoint: RoutePoint;
}

export class InputRoutePoint {
    parentRoutePointId: number;
    pointPostion: Postion;
}
