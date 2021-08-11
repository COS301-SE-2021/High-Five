import {Component, HostListener, OnInit} from '@angular/core';
import {LoadingController, Platform} from '@ionic/angular';
import {ScreenSizeServiceService} from './services/screen-size-service.service';
import {MsalService} from '@azure/msal-angular';
import {NavigationEnd, NavigationStart, Router, RouterEvent} from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent implements OnInit {
  private loading;
  isIframe: boolean = false;

  constructor(private platform: Platform, private screenSizeService: ScreenSizeServiceService, private msalService: MsalService,
              private router: Router, private loadingController: LoadingController) {
    this.initializeApp();
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
      if(msalService.instance.getAllAccounts().length>0){
        router.navigate(['/navbar/landing'])
      }
    });

  }

  //TODO : Look into better way to change platform, other than resizing
  //Source for idea : https://youtu.be/FVwuCO5vJxI
  @HostListener('window:resize', ['$event'])
  private onResize(event) {
    this.screenSizeService.onResize(event.target.innerWidth);
  }

  initializeApp() {
    this.platform.ready().then(() => {
      this.screenSizeService.onPlatformChange(this.platform);
    });
  }

  ngOnInit(): void {
    this.isIframe = window !== window.parent && !window.opener;

    this.msalService.instance.handleRedirectPromise().then(
      res => {
        if (res != null && res.account != null) {
          this.loading.present();
          this.msalService.instance.setActiveAccount(res.account);
          localStorage.setItem('jwt', res.idToken);
          this.router.navigate(['/navbar/landing']).then(() => {
            this.loading.dismiss();
          });
        }
      }
    );


  }

}
