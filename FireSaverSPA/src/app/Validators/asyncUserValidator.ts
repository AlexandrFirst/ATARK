import { AbstractControl, AsyncValidatorFn, ValidationErrors } from "@angular/forms";
import { Observable, of } from "rxjs";
import { map } from "rxjs/operators";
import { HttpUserService } from "../Services/httpUser.service";

export class AsyncUserValidator {
    static createUniqueUserValidator(userService: HttpUserService): AsyncValidatorFn {
        console.log("creating validator for mail");
        return (control: AbstractControl): Observable<ValidationErrors> | null => {

            console.log("validating mail on uniqueness");

            let checkVal = userService.IsMailUniq(control.value);
            return checkVal.pipe(map(respone => {
                console.log("isUNique: ", respone.isUnique)
                return respone.isUnique == true ? null : { isEmailUnique: false }
            }));
        }
    }

    static createCanUserBeResponsibleValidator(userService: HttpUserService): AsyncValidatorFn {
        console.log("creating validator for mail");
        return (control: AbstractControl): Observable<ValidationErrors> | null => {

            console.log("validating mail for making responsible");

            let checkVal = userService.CheckIfUserCanBeResponsible(control.value);
            return checkVal.pipe(map(respone => {
                console.log("can be responsible: ", respone.canBeResponsible)
                return respone.canBeResponsible == true ? null : { canBeResponsible: true }
            }));
        }
    }
}
