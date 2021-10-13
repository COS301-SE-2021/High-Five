import {Injectable} from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  CanActivateChild,
  CanLoad,
  Route, Router,
  RouterStateSnapshot,
  UrlSegment,
  UrlTree
} from '@angular/router';
import {Observable} from 'rxjs';
import {UsersService} from '../services/users/users.service';

@Injectable()

export class AdminGuard implements CanActivate, CanLoad, CanActivateChild {
  constructor(private usersService: UsersService, private router: Router) {
  }

  canActivate(route: ActivatedRouteSnapshot,
              state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

    if (!this.usersService.getIsAdmin()) {
      this.router.navigate(['navbar/tools/approved']);
      return false;
    }
    return true;
  }

  canLoad(route: Route,
          segments: UrlSegment[]): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

    if (!this.usersService.getIsAdmin()) {
      this.router.navigate(['navbar/tools/approved']);
      return false;
    }
    return true;


  }

  canActivateChild(childRoute: ActivatedRouteSnapshot,
                   state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (!this.usersService.getIsAdmin()) {
      this.router.navigate(['navbar/tools/approved']);
      return false;
    }
    return true;
  }

}
