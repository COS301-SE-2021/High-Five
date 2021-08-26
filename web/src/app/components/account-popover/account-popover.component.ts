import {Component, Input, OnInit} from '@angular/core';
import {MsalService} from '@azure/msal-angular';
import {Router} from '@angular/router';
import {ModalController} from '@ionic/angular';
import {AccountComponent} from '../account/account.component';

@Component({
  selector: 'app-account-popover',
  templateUrl: './account-popover.component.html',
  styleUrls: ['./account-popover.component.scss'],
})
export class AccountPopoverComponent implements OnInit {

  constructor(private msalService: MsalService, private router: Router, private modalController: ModalController) {
  }

  @Input() onClick = () => {
  };

  ngOnInit() {
  }

  public logout() {
    this.onClick();
    this.msalService.logoutPopup();
    this.router.navigate(['/welcome']).then(() => {
      localStorage.clear();
    });
  }

  /**
   * Function opens a modal containing the add pipeline component, which will allow the user to create a pipeline
   */
  public async displayAccountPreferencesModal() {
    const modal = await this.modalController.create({
      component: AccountComponent,
      cssClass: 'account-preferences-modal',
      showBackdrop: false,
      animated: true,
      backdropDismiss: true,
    });
    return modal.present();
  }

}
