import {Component, Input, OnInit} from '@angular/core';
import {AnalyzedVideoMetaData} from '../../models/analyzedVideoMetaData';
import {VideostreamCardComponent} from '../videostream-card/videostream-card.component';
import {ModalController} from '@ionic/angular';

@Component({
  selector: 'app-analyzed-videostore-card',
  templateUrl: './analyzed-videostore-card.component.html',
  styleUrls: ['./analyzed-videostore-card.component.scss'],
})
export class AnalyzedVideostoreCardComponent implements OnInit {
  @Input() analyzedVideo: AnalyzedVideoMetaData;

  constructor(private modalController: ModalController) {
  }

  ngOnInit() {
  }

  /**
   * This function creates a modal where the recorded drone footage can be
   * replayed to the user.
   */
  async playVideo() {
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
}
