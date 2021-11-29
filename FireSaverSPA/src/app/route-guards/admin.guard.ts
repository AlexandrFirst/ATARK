import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { AdminService } from "../Services/admin.service";

@Injectable({
    providedIn: 'root'
})
export class AdminGuard implements CanActivate {

    constructor(private adminService: AdminService,
        private router: Router,
        private toastr: ToastrService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {

        return this.adminService.checkAdminRights().pipe(map((data: any) => {
            if (data.message) {
                return true;
            }
            else {
                return false;
            }
        }))


    }
}