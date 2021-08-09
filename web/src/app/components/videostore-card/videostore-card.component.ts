import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {ModalController, Platform} from '@ionic/angular';
import {VideostreamCardComponent} from '../videostream-card/videostream-card.component';
import {VideoMetaData} from '../../models/videoMetaData';


@Component({
  selector: 'app-videostore-card',
  templateUrl: './videostore-card.component.html',
  styleUrls: ['./videostore-card.component.scss'],
})
export class VideostoreCardComponent implements OnInit {
  @Input() video: VideoMetaData;
  @Output() deleteVideo: EventEmitter<string> = new EventEmitter<string>();

  constructor(public platform: Platform, private modal: ModalController) {
  }

  ngOnInit() {
    //Nothing added here yet

  }

  /**
   * This function creates a modal where the recorded drone footage can be
   * replayed to the user.
   */
  async playVideo() {
    const videoModal = await this.modal.create({
      component: VideostreamCardComponent,
      componentProps: {
        modal: this.modal,
        videoUrl: this.video.url
      }
    });
    videoModal.style.backgroundColor = 'rgba(0,0,0,0.85)'; //make the background for the modal darker.

    await videoModal.present();
  }

  /**
   * Deletes this video by emitting the deleteVideo event
   */
  async onDeleteVideo() {
    this.deleteVideo.emit(this.video.id);
  }
}
