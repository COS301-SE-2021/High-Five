import {Component, OnInit} from '@angular/core';
import {UnreviewedTool} from '../../models/unreviewedTool';
import {UsersService} from '../../services/users/users.service';
import {AlertController, ToastController} from '@ionic/angular';
import {UserToolsService} from '../../services/user-tools/user-tools.service';

@Component({
  selector: 'app-unapproved-tools',
  templateUrl: './unapproved-tools.page.html',
  styleUrls: ['./unapproved-tools.page.scss'],
})
export class UnapprovedToolsPage implements OnInit {

  constructor(public usersService: UsersService, private toastController: ToastController,
              private alertController: AlertController, private userToolsService: UserToolsService) {
  }

  public unreviewedToolsTrackFn = (t, unreviewedTool) => unreviewedTool.id;

  ngOnInit() {
    this.usersService.fetchAllUnreviewedTools();
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

  public async onRemoveTool(toolId: string, toolType: string) {
    const alert = await this.alertController.create({
      header: 'Tool Deletion',
      message: `Are you sure you want to delete this tool ?`,
      animated: true,
      translucent: true,
      buttons: [
        {
          text: 'Cancel',
          handler: () => {
          }
        }, {
          text: `I'm Sure`,
          handler: () => {
            this.userToolsService.removeTool(toolId, toolType);
          }
        }
      ]
    });

    await alert.present();

  }
}
