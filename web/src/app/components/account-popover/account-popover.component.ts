import {Component, Input, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {ModalController} from '@ionic/angular';
import {AccountComponent} from '../account/account.component';
import {UserToolsService} from '../../services/user-tools/user-tools.service';
import {OAuthService} from 'angular-oauth2-oidc';

@Component({
  selector: 'app-account-popover',
  templateUrl: './account-popover.component.html',
  styleUrls: ['./account-popover.component.scss'],
})
export class AccountPopoverComponent implements OnInit {

  constructor(private router: Router, private modalController: ModalController,
              private userToolsService: UserToolsService, private oauthService: OAuthService) {
  }

  @Input() onClick = () => {
  };

  ngOnInit() {
    this.userToolsService.fetchAllTools();
  }

  public logout() {
    this.onClick();
    // this.msalService.logoutPopup();
    this.oauthService.postLogoutRedirectUri = window.location.origin;
    this.oauthService.logOut();
    // this.router.navigate(['/welcome']).then(() => {
    //   sessionStorage.clear();
    // });
    // this.
  }

  /**
   * Function opens a modal containing the add pipeline component, which will allow the user to create a pipeline
   */
  public async displayAccountPreferencesModal() {
    this.onClick();
    const modal = await this.modalController.create({
      component: AccountComponent,
      cssClass: 'accountPreferencesModal',
      showBackdrop: false,
      animated: true,
      backdropDismiss: true,
    });
    return modal.present();
  }

}
