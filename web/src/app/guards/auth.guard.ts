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
import {OAuthService} from 'angular-oauth2-oidc';

@Injectable()

export class AuthGuard implements CanActivate, CanLoad, CanActivateChild {
  constructor(private oauthService: OAuthService, private router: Router) {
  }

  canActivate(route: ActivatedRouteSnapshot,
              state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    const idToken = this.oauthService.hasValidIdToken();
    const accessToken = this.oauthService.hasValidAccessToken();
    if (!idToken || !accessToken) {
      this.router.navigate(['/welcome']);
      return (idToken && accessToken);

    }
    return (idToken && accessToken);
  }

  canLoad(route: Route,
          segments: UrlSegment[]): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

    const idToken = this.oauthService.hasValidIdToken();
    const accessToken = this.oauthService.hasValidAccessToken();
    if (!idToken || !accessToken) {
      this.router.navigate(['/welcome']);
      return (idToken && accessToken);

    }
    return (idToken && accessToken);


  }

  canActivateChild(childRoute: ActivatedRouteSnapshot,
                   state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    const idToken = this.oauthService.hasValidIdToken();
    const accessToken = this.oauthService.hasValidAccessToken();
    if (!idToken || !accessToken) {
      this.router.navigate(['/welcome']);
      return (idToken && accessToken);

    }
    return (idToken && accessToken);
  }

}
