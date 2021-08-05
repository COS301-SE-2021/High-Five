import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {AlertController, ModalController, Platform, ToastController} from '@ionic/angular';
import {VideostreamCardComponent} from '../videostream-card/videostream-card.component';
import {VideoMetaData} from '../../models/videoMetaData';
import {VideoStoreCardConstants} from '../../../constants/components/videostore-card-constants';
import {MediaService} from '../../services/media/media.service';

@Component({
  selector: 'app-videostore-card',
  templateUrl: './videostore-card.component.html',
  styleUrls: ['./videostore-card.component.scss'],
})
export class VideostoreCardComponent implements OnInit {
  @Input() video: VideoMetaData;
  @Output() onDeleteVideo: EventEmitter<string> = new EventEmitter<string>();

  constructor(public platform: Platform, private modal: ModalController,
              private mediaService: MediaService, private alertController: AlertController,
              private toastController: ToastController,
              private constants: VideoStoreCardConstants) {
  }

  ngOnInit() {
    //Nothing added here yet

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
    this.onDeleteVideo.emit(this.video.id);
  }
}
