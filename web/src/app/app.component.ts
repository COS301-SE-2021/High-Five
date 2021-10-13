import {Component, OnInit} from '@angular/core';
import {LoadingController, Platform} from '@ionic/angular';
import {NavigationEnd, NavigationStart, Router, RouterEvent} from '@angular/router';
import {environment} from '../environments/environment';
import {SnotifyPosition, SnotifyService} from 'ng-snotify';
import {OAuthService} from 'angular-oauth2-oidc';


@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent implements OnInit {
  private loading;

  constructor(private platform: Platform, private oauthService: OAuthService,
              private router: Router, private loadingController: LoadingController, private snotifyService: SnotifyService) {
    this.oauthService.loadDiscoveryDocument(environment.oauthConfig.action.loginDiscoveryDoc);
    this.oauthService.tryLoginImplicitFlow();
    this.snotifyService.setDefaults({
      toast: {
        timeout: 3000,
        bodyMaxLength: 200,
        titleMaxLength: 50,
        position: SnotifyPosition.leftBottom
      },
      global: {
        maxOnScreen: 5
      }
    });
    this.loadingController.create({
      animated: true,
      spinner: 'circles',
      backdropDismiss: false,
      message: 'Redirecting',
    }).then((overlay: HTMLIonLoadingElement) => {
      this.loading = overlay;
      router.events.subscribe((event: RouterEvent) => {
        if (event instanceof NavigationStart) {
          this.loading.present();
        }
        if (event instanceof NavigationEnd) {
          this.loading.dismiss();
        }
      });
    });

  }


  ngOnInit(): void {
  }

}
