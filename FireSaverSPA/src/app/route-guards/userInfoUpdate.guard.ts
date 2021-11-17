import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { AccountComponent } from "../Components/Account/Account.component";

@Injectable({
    providedIn: 'root'
})
export class UserInfoUpdateGuard implements CanDeactivate<AccountComponent> {
    canDeactivate(component: AccountComponent, currentRoute: ActivatedRouteSnapshot, currentState: RouterStateSnapshot, nextState?: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
        
        //TODO: check if we haven't saved changes, activate this guard
        
        throw new Error("Method not implemented.");
    }

}