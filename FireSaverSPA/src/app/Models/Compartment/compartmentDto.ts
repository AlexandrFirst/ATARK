import { TestInput } from "../TestModels/testInput";

export class CompartmentDto {
    id: number = 0;
    name: string;
    description: string;
    safetyRules: string;
    compartmentTest: TestInput;
}