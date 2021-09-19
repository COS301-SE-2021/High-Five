import {Component, OnInit} from '@angular/core';
import {LoadingController, Platform} from '@ionic/angular';
import {MsalService} from '@azure/msal-angular';
import {NavigationEnd, NavigationStart, Router, RouterEvent} from '@angular/router';
import {environment} from '../environments/environment';
import {SnotifyPosition, SnotifyService} from 'ng-snotify';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent implements OnInit {
  public isIframe = false;
  private loading;

  constructor(private platform: Platform, private msalService: MsalService,
              private router: Router, private loadingController: LoadingController, private snotifyService: SnotifyService) {
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
      if (msalService.instance.getAllAccounts().length > 0) {
        router.navigate(['/navbar/landing']);
      }
    });

  }


  ngOnInit(): void {
    this.isIframe = window !== window.parent && !window.opener;

    /**
     * The below is to catch the redirect of the msal service after successful authentication
     */
    this.msalService.instance.handleRedirectPromise().then(
      res => {
        if (res != null && res.account != null) {
          this.loading.present();
          this.msalService.instance.setActiveAccount(res.account);
          if (!environment.production) {
            console.log(res);
          }
          /**
           * We need to manually route the user to the correct url, as the msal service's function that should take
           * the redirect URI into account, doesn't work
           */
          this.router.navigate(['/navbar/landing']).then(() => {
            this.loading.dismiss();
          });
        }
      }
    );


  }

}
