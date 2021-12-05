import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { Observable } from "rxjs";
import { BaseHttpService } from "../Services/baseHttp.service";
import { HttpUserService } from "../Services/httpUser.service";

@Injectable({
    providedIn: 'root'
})
export class MainContentGuard implements CanActivate {

    constructor(private userService: HttpUserService,
        private toastrService: ToastrService,
        private router: Router) {
    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {

        var authData = this.userService.readAuthResponse();
        if (authData.token != "-1") {
            this.userService.CheckTokenValidity(authData.token).subscribe(response => {
                this.router.navigate(['main']);
                this.toastrService.success("Welcome")
            }, error => {
                this.toastrService.success("Login again please")
            })
        }

        return true;
    }

}