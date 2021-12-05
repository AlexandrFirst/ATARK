import { IoTNewPostion } from "../IoTService/ioTNewPostion";
import { TestInput } from "../TestModels/testInput";
import { UserInfoDto } from "../UserService/userInfoDto";

export class CompartmentDto {
    id: number = 0;
    name: string;
    description: string;
    safetyRules: string;
    compartmentTest: TestInput;
    ioTs: IoTNewPostion[] = [];
    inboundUsers: UserInfoDto[] = []
}