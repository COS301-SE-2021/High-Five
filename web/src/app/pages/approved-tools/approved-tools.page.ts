import {Component, OnInit} from '@angular/core';
import {UserToolsService} from '../../services/user-tools/user-tools.service';
import {CreateToolComponent} from '../../components/create-tool/create-tool.component';
import {AlertController, ModalController} from '@ionic/angular';

@Component({
  selector: 'app-approved-tools',
  templateUrl: './approved-tools.page.html',
  styleUrls: ['./approved-tools.page.scss'],
})
export class ApprovedToolsPage implements OnInit {

  constructor(public userToolsService: UserToolsService, private modalController: ModalController,
              private alertController: AlertController) {
  }

  public userToolsTrackFn = (t, userTool) => userTool.id;

  ngOnInit() {
  }

  public async addUserTool() {
   this.modalController.create({
      component: CreateToolComponent,
      cssClass: 'accountPreferencesModal',
      showBackdrop: false,
      animated: true,
      backdropDismiss: true
    }).then((c) => {
      c.present();
    });
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
