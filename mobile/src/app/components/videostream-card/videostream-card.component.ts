import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {VideoPreviewData} from "../../pages/videostore/videostore.page";
import {ModalController} from "@ionic/angular";
import {VideoPlayer} from "@ionic-native/video-player/ngx";
@Component({
  selector: 'app-videostream-card',
  templateUrl: './videostream-card.component.html',
  styleUrls: ['./videostream-card.component.scss'],
})
export class VideostreamCardComponent implements OnInit {
  @Input() modal: ModalController;
  constructor( private vPlayer : VideoPlayer) { }

  ngOnInit() {}

  /**
   * Dismisses the video playback modal.
   */
  async dismissModal() {
    await this.modal.dismiss()
  }

}
