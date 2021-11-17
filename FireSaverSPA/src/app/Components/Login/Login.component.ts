import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { LoginUserDto } from 'src/app/Models/UserService/loginUserDto';
import { RegistrationUserData } from 'src/app/Models/UserService/registrationUserData';
import { HttpUserService } from 'src/app/Services/httpUser.service';
import { LoaderSignServiceService } from 'src/app/Services/LoaderSignService.service';
import { PasswordConfirmValidator } from 'src/app/Validators/passwordConfirmValidator';
import { AsyncUniqMailValidator } from 'src/app/Validators/UniqEmailValidator';

@Component({
  selector: 'app-Login',
  templateUrl: './Login.component.html',
  styleUrls: ['./Login.component.scss']
})
export class LoginComponent implements OnInit {


  showPassword: boolean = true;

  loginForm: FormGroup;

  registerForm: FormGroup;

  get registrationName() {
    return this.registerForm.get('name');
  }

  get registrationSurname() {
    return this.registerForm.get('surname');
  }

  get registrationPatronymic() {
    return this.registerForm.get('patronymic');
  }

  get registrationMail() {
    return this.registerForm.get('mail');
  }

  get registrationTelNumber() {
    return this.registerForm.get('telNumber');
  }

  get registrationDob() {
    return this.registerForm.get('dob');
  }

  get registrationPassword() {
    return this.registerForm.get('password');
  }

  get registrationConfirmPassword() {
    return this.registerForm.get('confirmPassword');
  }

  get loginMail() {
    return this.loginForm.get('mail');
  }

  get loginPass() {
    return this.loginForm.get('password');
  }

  constructor(private userService: HttpUserService, 
              private toastr: ToastrService,
              private loaderSignService: LoaderSignServiceService) {

    this.loginForm = new FormGroup({
      mail: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required])
    });

    this.registerForm = new FormGroup({
      name: new FormControl('', [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(20)
      ]),
      surname: new FormControl('', [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(20)
      ]),
      patronymic: new FormControl('', [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(20)
      ]),
      mail: new FormControl('', [
        Validators.required,
        Validators.email
      ], [AsyncUniqMailValidator.createValidator(this.userService)]),
      telNumber: new FormControl('', [
        Validators.required,
        Validators.pattern('^\\+?3?8?(0[\\s\\.-]\\d{2}[\\s\\.-]\\d{3}[\\s\\.-]\\d{2}[\\s\\.-]\\d{2})$')
      ]),
      dob: new FormControl('', [
        Validators.required
      ]),
      password: new FormControl('', [
        Validators.required,
        Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&_])[A-Za-z\\d@$!%*?&_]{8,}$'),
        Validators.minLength(8),
        Validators.maxLength(30)
      ]),
      confirmPassword: new FormControl(''),
    }, PasswordConfirmValidator);


  }

  ngOnInit() {
    const switchers = document.querySelectorAll('.switcher')
    switchers.forEach(item => {
      item.addEventListener('click', function () {
        switchers.forEach(item => item.parentElement.classList.remove('is-active'))
        this.parentElement.classList.add('is-active')
      })
    })
  }


  showHidePasswordBtnClick() {
    this.showPassword = !this.showPassword;
    console.log("showHidePaasordLabelClick")
  }

  sendLoginData() {
    console.log(this.loginForm);
    if (this.loginForm.invalid) {
      this.toastr.error("Login form data is invalid")
    }
    else {
      this.loaderSignService.activate();

      this.userService.LoginUser({
        mail: this.loginMail.value,
        password: this.loginPass.value
      } as LoginUserDto).subscribe(response => {
        this.userService.writeAuthResponse(response);
        this.toastr.success("Welcome");
        //TODO: create redirection to main page

      }, error => {
        console.log(error);
        this.toastr.error("Error while signing up. Try again");        
        this.loaderSignService.deactivate();
      }, () =>{
        this.loaderSignService.deactivate();
      })
    }
  }

  sendRegisterData() {
    console.log(this.registerForm);
    if (this.registerForm.invalid) {
      this.toastr.error("Registration form data is invalid")
    }
    else {
      this.loaderSignService.activate();

      this.userService.SendUserRegistrationData({
        dob: this.registrationDob.value,
        mail: this.registrationMail.value,
        name: this.registrationName.value,
        password: this.registrationPassword.value,
        patronymic: this.registrationPatronymic.value,
        surname: this.registrationName.value,
        telephoneNumber: this.registrationTelNumber.value
      } as RegistrationUserData).subscribe(response => {
        console.log(response)
        this.toastr.success("You are sucessfully registered")
        this.registerForm.reset();
        const switchers = document.querySelectorAll('.switcher');
        (switchers[0] as HTMLElement).click();

      }, error => {
        console.log(error)
        this.toastr.error("Error while creating user. Try again")
        this.loaderSignService.deactivate();
      }, () => {
        this.loaderSignService.deactivate();
      });
    }
  }
}
