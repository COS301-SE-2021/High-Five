import {Component, OnInit} from '@angular/core';
import {ModalController} from '@ionic/angular';
import {UsersService} from '../../services/users/users.service';
import {User} from '../../models/user';
import {UserToolsService} from '../../services/user-tools/user-tools.service';
import {OAuthService} from 'angular-oauth2-oidc';
import {environment} from '../../../environments/environment';


@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss'],
})
export class AccountComponent implements OnInit {
  public option: string;


  constructor(private modalController: ModalController,
              public usersService: UsersService, public userToolsService: UserToolsService,
              public oauthService: OAuthService) {

    this.option = 'details';
  }

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
    });
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


  public editUserProfile() {
    this.oauthService.loadDiscoveryDocument(environment.oauthConfig.action.editDiscoveryDoc).then(() => {
      this.oauthService.initLoginFlow();
    });
  }

}
