import {Component, Input, OnInit} from '@angular/core';
import {VideoPreviewData} from '../../pages/videostore/videostore.page';
import {ModalController, Platform} from '@ionic/angular';
import {VideostreamCardComponent} from '../videostream-card/videostream-card.component';
import {ThemeService} from '../../services/theme/theme.service';

@Component({
  selector: 'app-videostore-card',
  templateUrl: './videostore-card.component.html',
  styleUrls: ['./videostore-card.component.scss'],
})
export class VideostoreCardComponent implements OnInit {
  @Input() data: VideoPreviewData;  //be specific later
  isDarkMode: boolean;
  constructor(public platform: Platform, private modal: ModalController, private themeService: ThemeService) {
    this.isDarkMode= themeService.isDarkMode();
  }

  ngOnInit() {
  }

  /**
   * This function creates a modal where the recorded drone footage can be
   * replayed to the user.
   */
  async playVideo() {
    const videoModal = await this.modal.create({
      component: VideostreamCardComponent,
      componentProps: {
        modal: this.modal
      }
    });
    videoModal.style.backgroundColor = 'rgba(0,0,0,0.8)' ;//make the background for the modal darker.

    await videoModal.present();
  }
}
