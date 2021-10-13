import {Component, Input, OnInit} from '@angular/core';
import {AnalyzedVideoMetaData} from '../../models/analyzedVideoMetaData';
import {VideostreamCardComponent} from '../videostream-card/videostream-card.component';
import {AlertController, ModalController} from '@ionic/angular';
import {AnalyzedVideosService} from '../../services/analyzed-videos/analyzed-videos.service';

@Component({
  selector: 'app-analyzed-videostore-card',
  templateUrl: './analyzed-videostore-card.component.html',
  styleUrls: ['./analyzed-videostore-card.component.scss'],
})
export class AnalyzedVideostoreCardComponent implements OnInit {
  @Input() analyzedVideo: AnalyzedVideoMetaData;

  constructor(private modalController: ModalController, private analyzedVideoService: AnalyzedVideosService,
              private alertController: AlertController) {
  }

  ngOnInit() {
  }

  /**
   * This function creates a modal where the recorded drone footage can be
   * replayed to the user.
   */
  public async playVideo() {
    const videoModal = await this.modalController.create({
      component: VideostreamCardComponent,
      componentProps: {
        modal: this.modalController,
        videoUrl: this.analyzedVideo.url
      }
    });
    videoModal.style.backgroundColor = 'rgba(0,0,0,0.85)'; //make the background for the modal darker.
    await videoModal.present();
  }


  public async onDeleteAnalyzedVideo() {
    const alert = await this.alertController.create({
      header: 'Analysed Video Deletion',
      message : `Are you sure you want to delete this analysed video ?`,
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
            this.analyzedVideoService.deleteAnalyzedVideo(this.analyzedVideo.id);
          }
        }
      ]
    });

    await alert.present();
  }
}
