import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";



export function FloorLevelValidator(takenFloorLevels: number[]): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {

        const selectedLevel = Number.parseInt(control?.value);

        if (!takenFloorLevels.includes(selectedLevel)) {
            return null
        }
        else {
            return {
                canFloorBeSelected: true
            }
        }
    }
}