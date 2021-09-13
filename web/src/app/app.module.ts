import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {RouteReuseStrategy} from '@angular/router';
import {IonicModule, IonicRouteStrategy} from '@ionic/angular';
import {AppComponent} from './app.component';
import {AppRoutingModule} from './app-routing.module';
import {HttpClientModule} from '@angular/common/http';
import {VideoPlayer} from '@ionic-native/video-player/ngx';
import {PipelinesService} from './apis/pipelines.service';
import {MsalGuard, MsalModule} from '@azure/msal-angular';
import {InteractionType, PublicClientApplication} from '@azure/msal-browser';
import {environment} from '../environments/environment';
import {MediaStorageService} from './apis/mediaStorage.service';
import {AnalysisService} from './apis/analysis.service';
import {UserService} from './apis/user.service';
import {SnotifyModule, SnotifyService, ToastDefaults} from 'ng-snotify';
import {ToolsService} from './apis/tools.service';


@NgModule({
  declarations: [AppComponent],
  entryComponents: [],
  imports: [BrowserModule, IonicModule.forRoot(), AppRoutingModule, SnotifyModule, HttpClientModule,
    MsalModule.forRoot(new PublicClientApplication({
      auth: {
        clientId: environment.clientId,
        authority: environment.b2cPolicies.authorities.signUpSignIn.authority,
        knownAuthorities: [environment.b2cPolicies.authorityDomain],
        redirectUri: environment.redirectUri,
        postLogoutRedirectUri: environment.postLogoutRedirectUri
      },
      cache: {
        cacheLocation: 'sessionStorage',
        storeAuthStateInCookie: false
      }
    }), {
      interactionType: InteractionType.Redirect,
      authRequest: {
        scopes: ['user.read']
      },
      loginFailedRoute: environment.postLogoutRedirectUri
    }, null)],
  providers: [{
    provide: RouteReuseStrategy,
    useClass: IonicRouteStrategy
  }, VideoPlayer, PipelinesService, MsalGuard, MediaStorageService, AnalysisService, UserService, ToolsService, {
    provide: 'SnotifyToastConfig', useValue: ToastDefaults,
  },
    SnotifyService],
  bootstrap: [AppComponent],
})
export class AppModule {
}
