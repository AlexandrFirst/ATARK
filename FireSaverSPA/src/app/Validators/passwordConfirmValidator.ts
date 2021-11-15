import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export const PasswordConfirmValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
    
    const pass = control.get('password');
    const confirmPass = control.get('confirmPassword');

    if (pass && confirmPass && pass.value === confirmPass.value) {
        return null
    }
    else {
        return {
            passwordConfirmation: true
        }
    }
};