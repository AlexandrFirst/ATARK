import { AbstractControl, AsyncValidatorFn, ValidationErrors } from "@angular/forms";
import { Observable, of } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { HttpBuildingService } from "../Services/httpBuilding.service";

export class AsyncAddressValidator {
    static createAddressValidator(buildingService: HttpBuildingService): AsyncValidatorFn {
        console.log('creating validator for address');
        return (control: AbstractControl): Observable<ValidationErrors> | null => {

            return buildingService.validateBuildingAdress(control.value).pipe(map(response => {
                return null;
            }), catchError(err => of({ addressCheck: false })))
        };
    }
}