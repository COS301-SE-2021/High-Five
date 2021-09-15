import {Component, OnInit} from '@angular/core';
import {ModalController} from '@ionic/angular';
import {MsalService} from '@azure/msal-angular';
import {UsersService} from '../../services/users/users.service';
import {User} from '../../models/user';
import {UserToolsService} from '../../services/user-tools/user-tools.service';
import {CreateToolComponent} from '../create-tool/create-tool.component';
import {OAuthService} from 'angular-oauth2-oidc';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss'],
})
export class AccountComponent implements OnInit {
  public option: string;


  constructor(private modalController: ModalController, public msalService: MsalService,
              public usersService: UsersService, public userToolsService: UserToolsService,
              private oauthService: OAuthService) {

    this.option = 'details';
  }

  public userToolsTrackFn = (t, userTool) => userTool.id;

  public usersTrackFn = (u, user) => user.id;

  ngOnInit() {
  }


  public async close() {
    await this.modalController.dismiss();
  }

  public async requestToRemoveUser(user: User) {
    console.log('Request sent to remove user');
  }

  public async purgeUserMedia(user: User) {
    this.usersService.purgeMedia(user.id).then(() => {
      }
    );
  }

  public async changeUserAdminStatus(event: Event, user: User) {
    /* eslint-disable */
    const checked: boolean = event['detail']['checked'];
    /* eslint-enable */
    if (checked) {
      await this.usersService.upgradeToAdmin(user.id);
    } else {
      await this.usersService.revokeAdmin(user.id);
    }
  }

  public async purgeOwnMedia() {
    await this.usersService.purgeOwnMedia();
  }

  public async addUserTool() {
    this.modalController.dismiss({dismissed: true}).then(() => this.modalController.create({
      component: CreateToolComponent,
      cssClass: 'accountPreferencesModal',
      showBackdrop: false,
      animated: true,
      backdropDismiss: true
    }).then((c) => {
      c.present();
    }));
  }

  public editUserProfile() {
    this.oauthService.initLoginFlowInPopup().then((val) => {
      console.log(val);
    });
  }
}
