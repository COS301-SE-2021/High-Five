import {Component, OnInit} from '@angular/core';
import {ModalController, ToastController} from '@ionic/angular';
import {MsalService} from '@azure/msal-angular';
import {UsersService} from '../../services/users/users.service';
import {User} from '../../models/user';
import {UserToolsService} from '../../services/user-tools/user-tools.service';
import {CreateToolComponent} from '../create-tool/create-tool.component';
import {OAuthService} from 'angular-oauth2-oidc';
import {UnreviewedTool} from '../../models/unreviewedTool';


@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss'],
})
export class AccountComponent implements OnInit {
  public option: string;


  constructor(private modalController: ModalController, public msalService: MsalService,
              public usersService: UsersService, public userToolsService: UserToolsService,
              private oauthService: OAuthService, private toastController: ToastController) {

    this.option = 'details';
  }

  public userToolsTrackFn = (t, userTool) => userTool.id;
  public unreviewedToolsTrackFn = (t, unreviewedTool) => unreviewedTool.id;

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
    this.oauthService.initLoginFlow();
  }

  public getToolName(val: string): string {
    return this.userToolsService.userTools.filter((t) => t.toolId === val)[0].toolName;
  }

  public rejectTool(unapprovedTool: UnreviewedTool) {
    this.usersService.rejectTool(unapprovedTool);
  }

  public approveTool(unapprovedTool: UnreviewedTool) {
    this.usersService.approveTool(unapprovedTool);
  }

  public refreshUnapproved() {
    this.usersService.fetchAllUnreviewedTools().then(() => {
      this.toastController.create({
        message: `Unapproved tools refreshed`,
        duration: 2000,
        translucent: true,
        position: 'bottom'
      }).then((toast) => {
        toast.present();
      });
    });
  }

  downloadOnnx(unapprovedTool: UnreviewedTool) {
    window.open(unapprovedTool.toolModel);
  }

  downloadDll(unapprovedTool: UnreviewedTool) {
    window.open(unapprovedTool.toolDll);

  }
}
