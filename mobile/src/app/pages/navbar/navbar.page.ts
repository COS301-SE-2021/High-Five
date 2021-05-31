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

  private navPages;
  homeLink = "active-link";
  analyticsLink = "link";
  videoLink = "link";

  isDesktop: boolean;
  constructor(private screenSizeService: ScreenSizeServiceService, private nav : Router) {
    this.screenSizeService.isDesktopView().subscribe(isDesktop=>{
      this.isDesktop = isDesktop;
    });
  }
  ngOnInit() {
    this.navPages = {
      'homeNav' : this.homeNav,
      'analyticsNav' : this.analyticsNav,
      'videoNav' : this.videoNav
    }
    console.log(this.homeNav)
  }

  navigateTo(url : String, tab : String) {
    this.nav.navigate([url]);


    for (let key in this.navPages) {
      console.log(this.navPages[key])
      let value = this.navPages[key];
      if (key !== tab) {
        value.setAttribute('class', 'link');
      } else {
        value.setAttribute('class', 'active-link');
      }
    }
  }

}
