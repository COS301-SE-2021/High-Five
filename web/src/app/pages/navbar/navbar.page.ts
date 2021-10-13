import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {PopoverController} from '@ionic/angular';
import {NavbarMediaPopoverComponent} from '../../components/navbar-media-popover/navbar-media-popover.component';
import {AccountPopoverComponent} from '../../components/account-popover/account-popover.component';
import {OAuthService} from 'angular-oauth2-oidc';
import {UsersService} from '../../services/users/users.service';
import {WebsocketService} from '../../services/websocket/websocket.service';
import {UserPreferencesService} from '../../services/user-preferences/user-preferences.service';


@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.page.html',
  styleUrls: ['./navbar.page.scss'],
})
export class NavbarPage implements OnInit {

  constructor(private router: Router, private popoverController: PopoverController,
              private oauthService: OAuthService,
              private usersService: UsersService, private websocketService: WebsocketService,
              private userPreferences: UserPreferencesService) {
    this.oauthService.setupAutomaticSilentRefresh();

  }

  ngOnInit() {
    //Nothing added here yet
  }

  /**
   * Function that calls the MSAL service's logout method to logout and then clears the localstorage
   */
  logout() {
    // this.msalService.logout();
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
