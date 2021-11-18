export class UserInfoDto
{
    id: number = -1;
    name: string = "";
    surname: string = "";
    patronymic: string = "";
    mail: string = "";
    telephoneNumber: string = "";
    dob: Date = new Date();
}