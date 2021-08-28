import {Component, OnInit} from '@angular/core';
import {ScreenSizeServiceService} from '../../services/screen-size-service.service';
import {Router} from '@angular/router';
import {MsalService} from '@azure/msal-angular';
import {PopoverController} from '@ionic/angular';
import {NavbarMediaPopoverComponent} from '../../components/navbar-media-popover/navbar-media-popover.component';
import {AccountPopoverComponent} from '../../components/account-popover/account-popover.component';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.page.html',
  styleUrls: ['./navbar.page.scss'],
})
export class NavbarPage implements OnInit {
  isDesktop: boolean;
  constructor(private screenSizeService: ScreenSizeServiceService, private router: Router,
              private msalService: MsalService, private popoverController: PopoverController) {
    this.screenSizeService.isDesktopView().subscribe(isDesktop => {
      this.isDesktop = isDesktop;
    });

  }

  ngOnInit() {
    //Nothing added here yet

  }

  /**
   * Function that calls the MSAL service's logout popup method to logout and then clears the localstorage
   */
  logout() {
    this.msalService.logoutPopup();
    this.router.navigate(['/welcome']).then(() => {
      localStorage.clear();
    });
  }


  /**
   * This function will display a popover containing the different media type pages' link to which the user can navigate
   *
   * @param ev, the event which calls this function, needed by the popoverController to create a popover
   */
  async displayMediaPopover(ev: any) {
    const popoverComponent = await this.popoverController.create({
      component: NavbarMediaPopoverComponent,
      cssClass: 'navBarMediaPopover',
      animated: true,
      translucent: true,
      backdropDismiss: true,
      event: ev,
      showBackdrop: false,
      componentProps: {
        onClick: () => {
          popoverComponent.dismiss();
        }
      }
    });
    return await popoverComponent.present();

  }


  async displayAccountPopover(ev: any) {
    const popoverComponent = await this.popoverController.create({
      component: AccountPopoverComponent,
      cssClass: 'accountOptionsPopover',
      animated: true,
      translucent: true,
      backdropDismiss: true,
      event: ev,
      showBackdrop: false,
      componentProps: {
        onClick: () => {
          popoverComponent.dismiss();
        }
      }
    });
    return await popoverComponent.present();
  }
}
