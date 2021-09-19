import {Component, Input, OnInit} from '@angular/core';
import {MsalService} from '@azure/msal-angular';
import {Router} from '@angular/router';
import {ModalController} from '@ionic/angular';
import {AccountComponent} from '../account/account.component';
import {UserToolsService} from '../../services/user-tools/user-tools.service';

@Component({
  selector: 'app-account-popover',
  templateUrl: './account-popover.component.html',
  styleUrls: ['./account-popover.component.scss'],
})
export class AccountPopoverComponent implements OnInit {

  constructor(private msalService: MsalService, private router: Router, private modalController: ModalController,
              private userToolsService: UserToolsService) {
  }

  @Input() onClick = () => {
  };

  ngOnInit() {
    this.userToolsService.fetchAllTools();
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
