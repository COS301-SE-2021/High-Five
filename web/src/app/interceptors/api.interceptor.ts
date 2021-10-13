import {Injectable} from '@angular/core';
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from '../../environments/environment';
import {OAuthService} from 'angular-oauth2-oidc';


@Injectable()
export class ApiInterceptor implements HttpInterceptor {
  constructor(private oauthService: OAuthService) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (req.url.includes(environment.apiEndpoint)) {
      if (!this.oauthService.hasValidAccessToken() ) {
        this.oauthService.silentRefresh();
        // eslint-disable-next-line @typescript-eslint/naming-convention
        const Authorization = 'Bearer ' + this.oauthService.getAccessToken();
        // eslint-disable-next-line @typescript-eslint/naming-convention
        return next.handle(req.clone({setHeaders: {Authorization}}));
      } else {
        // eslint-disable-next-line @typescript-eslint/naming-convention
        const Authorization = 'Bearer ' + this.oauthService.getAccessToken();
        // eslint-disable-next-line @typescript-eslint/naming-convention
        return next.handle(req.clone({setHeaders: {Authorization}}));
      }
    }
    return next.handle(req);
  }
}
