export class LoginUserDto {
    mail: string;
    password: string;
 }

 export class ResponseLoginDto{
     userId: number;
     token: string;
     roles: string[];
     responsibleBuildingId: number
 }