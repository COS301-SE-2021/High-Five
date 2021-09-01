import {Component, OnInit} from '@angular/core';
import {ModalController} from '@ionic/angular';
import {MsalService} from '@azure/msal-angular';
import {UsersService} from '../../services/users/users.service';
import {User} from '../../models/user';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss'],
})
export class AccountComponent implements OnInit {
  public option: string;


  constructor(private modalController: ModalController, public msalService: MsalService, public usersService: UsersService) {
    this.option = 'details';
  }

  public usersTrackFn = (u, user) => user.id;

  ngOnInit() {}


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
}
