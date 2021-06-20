import {Component, Input, OnInit} from '@angular/core';
import {AlertController, ModalController, Platform, ToastController} from '@ionic/angular';
import {VideostreamCardComponent} from '../videostream-card/videostream-card.component';
import {VideoMetaData} from '../../models/videoMetaData';
import {VideouploadService} from '../../services/videoupload/videoupload.service';
import {VideoStoreCardConstants} from "../../../constants/components/videostore-card-constants";

@Component({
  selector: 'app-videostore-card',
  templateUrl: './videostore-card.component.html',
  styleUrls: ['./videostore-card.component.scss'],
})
export class VideostoreCardComponent implements OnInit {
  @Input() data: VideoMetaData;
  @Input() deleter: any;

  constructor(public platform: Platform, private modal: ModalController,
              private videoService: VideouploadService, private alertController: AlertController,
              private toastController: ToastController,
              private constants: VideoStoreCardConstants) { }

  ngOnInit() {
  }

  /**
   * This function creates a modal where the recorded drone footage can be
   * replayed to the user.
   */
  async playVideo(vidId: string) {
    const videoModal = await this.modal.create({
      component: VideostreamCardComponent,
      componentProps: {
        modal: this.modal,
        vidId
      }
    });
    videoModal.style.backgroundColor = 'rgba(0,0,0,0.85)'; //make the background for the modal darker.

    await videoModal.present();
  }

  /**
   * Deletes a video by the ID passed to the function.
   *
   * @param vidId
   */
  async deleteVideo(vidId: string) {
    const alert = await this.alertController.create({
      cssClass: 'alerter',
      header: this.constants.alertLabels.header,
      message: this.constants.alertLabels.message,
      buttons: [
        {
          text: this.constants.alertLabels.buttonsYes.text,
          role: this.constants.alertLabels.buttonsYes.role
        }, {
          text: this.constants.alertLabels.buttonsNo.text,
          role: this.constants.alertLabels.buttonsNo.role
        }
      ]
    });

    await alert.present();

    const { role } = await alert.onDidDismiss();

    if (role === this.constants.alertLabels.buttonsYes.role) {
      this.videoService.deleteVideo(vidId, async data => {
        console.log(data);
        const toast = await this.toastController.create({
          cssClass: 'alert-style',
          header: this.constants.toastLabels.header,
          message: this.constants.toastLabels.message,
          buttons: this.constants.toastLabels.buttons
        });

        await toast.present();
        this.deleter(vidId);
      });
    }
  }
}
