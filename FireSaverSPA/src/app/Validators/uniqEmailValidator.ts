import { AbstractControl, AsyncValidatorFn, ValidationErrors } from "@angular/forms";
import { Observable, of } from "rxjs";
import { map } from "rxjs/operators";
import { HttpUserService } from "../Services/httpUser.service";

export class AsyncUniqMailValidator {
    static createValidator(userService: HttpUserService): AsyncValidatorFn {
        console.log("creating validator for mail");
        return (control: AbstractControl): Observable<ValidationErrors> | null => {

            console.log("validating");

            let checkVal = userService.IsMailUniq(control.value);
            return checkVal.pipe(map(respone => {
                console.log("isUNique: ", respone.isUnique)
                return respone.isUnique == true ? null : { isEmailUnique: false }
            }));
        }
    }
}
