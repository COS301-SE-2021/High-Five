import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {RouteReuseStrategy} from '@angular/router';
import {IonicModule, IonicRouteStrategy} from '@ionic/angular';
import {AppComponent} from './app.component';
import {AppRoutingModule} from './app-routing.module';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
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
import {LivestreamService} from './apis/livestream.service';
import {AuthConfig, OAuthModule} from 'angular-oauth2-oidc';
import {AuthGuard} from './guards/auth.guard';
import {AuthLoginGuard} from './guards/auth-login.guard';
import {ApiInterceptor} from './interceptors/api.interceptor';

const oauthConfig: AuthConfig = {
  issuer: environment.oauthConfig.issuer,
  redirectUri: window.location.origin+'/navbar/landing',
  clientId: environment.clientId,
  scope: environment.oauthConfig.scope,
  strictDiscoveryDocumentValidation: false,
  useSilentRefresh: true,
  sessionChecksEnabled: true,
  silentRefreshTimeout: 5000,
};


@NgModule({
  declarations: [AppComponent],
  entryComponents: [],
  imports: [BrowserModule, IonicModule.forRoot(), AppRoutingModule, SnotifyModule, HttpClientModule,
    OAuthModule.forRoot(),],
  providers: [
    {provide: AuthConfig, useValue: oauthConfig},
    AuthGuard,
    AuthLoginGuard,
    {provide: RouteReuseStrategy, useClass: IonicRouteStrategy},
    VideoPlayer, PipelinesService, MediaStorageService, AnalysisService, UserService, ToolsService, LivestreamService,
    {provide: 'SnotifyToastConfig', useValue: ToastDefaults},
    {provide: HTTP_INTERCEPTORS, useClass: ApiInterceptor, multi: true},
    SnotifyService],
  bootstrap: [AppComponent],
})
export class AppModule {
}
