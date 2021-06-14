import {Component, OnInit, ViewChild} from '@angular/core';
import {ScreenSizeServiceService} from '../../services/screen-size-service.service';
import {IonButton} from '@ionic/angular';
import {Navigation, Router} from '@angular/router';
import {ThemeService} from '../../services/theme/theme.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.page.html',
  styleUrls: ['./navbar.page.scss'],
})
export class NavbarPage implements OnInit {

  @ViewChild('homeNav') homeNav: HTMLIonButtonElement;
  @ViewChild('analyticsNav') analyticsNav: HTMLIonButtonElement;
  @ViewChild('videoNav') videoNav: HTMLIonButtonElement;
  @ViewChild('controlsNav') controlsNav: HTMLIonButtonElement;
  homeLink = ['active-link'];
  analyticsLink = ['link'];
  videoLink = ['link'];
  controlsLink = ['link'];

  isDesktop: boolean;
  darkMode: boolean ;
  private navPages;

  //These links are arrays so that when the content is changed, it is shown in the HTML

  constructor(private screenSizeService: ScreenSizeServiceService, private nav: Router, private themeService: ThemeService) {
    this.screenSizeService.isDesktopView().subscribe(isDesktop=>{
      this.isDesktop = isDesktop;
    });
    this.navPages = {
      homeNav : this.homeLink,
      analyticsNav : this.analyticsLink,
      videoNav : this.videoLink,
      controlsNav : this.controlsLink
    };
    this.darkMode=this.themeService.isDarkMode();
  }
  ngOnInit() {
  }

  /**
   * This function will navigate the application to the provided url, as well as change the styling of the
   * navigation buttons to communicate to the user that they are at that url.
   *
   * @param url The url to navigate to
   * @param tab The navigation button to set to be active
   */
  navigateTo(url: string, tab: string) {
    for (const key in this.navPages) {
      const value = this.navPages[key];
      if (key !== tab) {
        value[0] = 'link';
      } else {
        value[0] = 'active-link';
      }
    }
    this.nav.navigate([url]);
  }

  changeThemeMode(){
    this.themeService.toggleMode();
  }

}
