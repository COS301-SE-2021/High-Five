import {Injectable} from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  CanActivateChild,
  CanLoad,
  Route,
  RouterStateSnapshot,
  UrlSegment,
  UrlTree
} from '@angular/router';
import {Observable} from 'rxjs';
import {OAuthService} from 'angular-oauth2-oidc';

@Injectable()

export class AuthLoginGuard implements CanActivate, CanLoad, CanActivateChild {
  constructor(private oauthService: OAuthService) {
  }

  canActivate(route: ActivatedRouteSnapshot,
              state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    //   return this.authService.isDoneLoading$.pipe(
    //     filter(isDone => isDone),
    //     switchMap(_ => this.authService.isAuthenticated$),
    //     tap(isAuthenticated => isAuthenticated || this.authService.login(state.url)),
    //   );
    return true;

  }

  canLoad(route: Route,
          segments: UrlSegment[]): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    // return this.authService.isDoneLoading$.pipe(
    //   filter(isDone => isDone),
    //   switchMap(_ => this.authService.isAuthenticated$),
    //   tap(isAuthenticated => isAuthenticated || this.authService.login()),
    // );
    return true;

  }

  canActivateChild(childRoute: ActivatedRouteSnapshot,
                   state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    // return this.authService.isDoneLoading$.pipe(
    //   filter(isDone => isDone),
    //   switchMap(_ => this.authService.isAuthenticated$),
    //   tap(isAuthenticated => isAuthenticated || this.authService.login(state.url)),
    // );
    return true;

  }

}
