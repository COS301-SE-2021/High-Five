import {Component, OnInit} from '@angular/core';
import {ModalController} from '@ionic/angular';
import {MsalService} from '@azure/msal-angular';
import {UsersService} from '../../services/users/users.service';
import {User} from '../../models/user';
import {SnotifyService} from 'ng-snotify';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss'],
})
export class AccountComponent implements OnInit {
  public option: string;


  constructor(private modalController: ModalController, public msalService: MsalService, public usersService: UsersService,
              private snotifyService: SnotifyService) {
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
        this.snotifyService.success(user.displayName + ' media purged ', 'Media Purged');
      }
    );
  }

  public async changeUserAdminStatus(event: Event, user: User) {
    /* eslint-disable */
    const checked: boolean = event['detail']['checked'];
    /* eslint-enable */
    if (checked) {
      console.log('making admin');
      this.usersService.upgradeToAdmin(user.id).then(() => {
        this.snotifyService.success(user.displayName + ' has been upgraded to admin','Upgrade Successful');
      });
    } else {
      console.log('unmaking admin');

    }
  }


}
