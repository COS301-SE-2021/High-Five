import {Component, HostListener, OnInit} from '@angular/core';
import {Platform} from '@ionic/angular';
import {ScreenSizeServiceService} from './services/screen-size-service.service';
import {MsalService} from '@azure/msal-angular';
import {Router} from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent implements OnInit {
  constructor(private platform: Platform, private screenSizeService: ScreenSizeServiceService, private msalService: MsalService,
              private router: Router) {
    this.initializeApp();
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
    this.msalService.instance.handleRedirectPromise().then(
      res => {
        if (res != null && res.account != null) {
          this.msalService.instance.setActiveAccount(res.account);
          localStorage.setItem('jwt', res.idToken);
          this.router.navigate(['/navbar/landing']);
        }
      }
    );
  }
}
