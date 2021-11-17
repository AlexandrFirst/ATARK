import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivateChild, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { Observable } from "rxjs";
import { BaseHttpService } from "../Services/baseHttp.service";

@Injectable({
    providedIn: 'root'
})
export class LoginGuard implements CanActivateChild {

    constructor(private baseHttpService: BaseHttpService,
        private toastrService: ToastrService,
        private router: Router) { }

    canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {

        var authData = this.baseHttpService.readAuthResponse();
        if (authData.userId == -1) {
            this.router.navigate(['/']);
            this.toastrService.error("Login first!");
            return false;
        }
        return true;
    }

}