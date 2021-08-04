import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree} from '@angular/router';
import {Observable} from 'rxjs';
import {MsalService} from '@azure/msal-angular';

@Injectable({
  providedIn: 'root'
})
export class MsalGuard implements CanActivate {

  constructor(private msalService: MsalService) {
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree> | boolean | UrlTree {
    console.log(this.msalService.instance.getActiveAccount() != null);
    console.log(this.msalService.instance.getActiveAccount().name);
    return this.msalService.instance.getActiveAccount() != null;

  }
}
