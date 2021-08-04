import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {RouteReuseStrategy} from '@angular/router';

import {IonicModule, IonicRouteStrategy} from '@ionic/angular';

import {AppComponent} from './app.component';
import {AppRoutingModule} from './app-routing.module';
import {HttpClientModule} from '@angular/common/http';
import {VideoPlayer} from '@ionic-native/video-player/ngx';
import {PipelinesService} from './apis/pipelines.service';
import {MsalModule} from '@azure/msal-angular';
import {PublicClientApplication} from '@azure/msal-browser';
import {MsalGuard} from './services/msal-guard/msal-guard';
import {environment} from '../environments/environment';

const isIE = window.navigator.userAgent.indexOf('MSIE ') > -1 || window.navigator.userAgent.indexOf('Trident/') > -1;


@NgModule({
  declarations: [AppComponent],
  entryComponents: [],
  imports: [BrowserModule, IonicModule.forRoot(), AppRoutingModule, HttpClientModule, MsalModule.forRoot(new PublicClientApplication({
    auth: {
      clientId: environment.clientId,
      authority: environment.b2cPolicies.authorities.signUpSignIn.authority,
      knownAuthorities: [environment.b2cPolicies.authorityDomain],
      redirectUri: environment.redirectUri,
    },
    cache: {
      cacheLocation: 'sessionStorage',
      storeAuthStateInCookie: isIE
    }
  }), null, null)],
  providers: [{
    provide: RouteReuseStrategy,
    useClass: IonicRouteStrategy
  }, VideoPlayer, PipelinesService, MsalGuard],
  bootstrap: [AppComponent],
})
export class AppModule {
}
