import {Component, OnInit, ViewChild} from '@angular/core';
import {ScreenSizeServiceService} from '../../services/screen-size-service.service';
import {IonButton} from "@ionic/angular";
import {Navigation, Router} from "@angular/router";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.page.html',
  styleUrls: ['./navbar.page.scss'],
})
export class NavbarPage implements OnInit {

  @ViewChild('homeNav') homeNav : HTMLIonButtonElement;
  @ViewChild('analyticsNav') analyticsNav : HTMLIonButtonElement;
  @ViewChild('videoNav') videoNav : HTMLIonButtonElement;
  @ViewChild('controlsNav') controlsNav : HTMLIonButtonElement;

  private navPages;
  homeLink = ["active-link"];
  analyticsLink = ["link"];
  videoLink = ["link"];
  controlsLink = ["link"];

  isDesktop: boolean;
  constructor(private screenSizeService: ScreenSizeServiceService, private nav : Router) {
    this.screenSizeService.isDesktopView().subscribe(isDesktop=>{
      this.isDesktop = isDesktop;
    });
    this.navPages = {
      'homeNav' : this.homeLink,
      'analyticsNav' : this.analyticsLink,
      'videoNav' : this.videoLink,
      'controlsNav' : this.controlsLink
    }
  }
  ngOnInit() {
  }

  navigateTo(url : String, tab : String) {
    for (let key in this.navPages) {
      let value = this.navPages[key];
      if (key !== tab) {
        value[0] = 'link';
      } else {
        value[0] = 'active-link';
      }
    }
    this.nav.navigate([url]);
  }

}
