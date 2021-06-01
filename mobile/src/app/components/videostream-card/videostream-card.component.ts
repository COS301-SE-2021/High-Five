import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {VideoPreviewData} from "../../pages/videostore/videostore.page";
import {ModalController} from "@ionic/angular";

@Component({
  selector: 'app-videostream-card',
  templateUrl: './videostream-card.component.html',
  styleUrls: ['./videostream-card.component.scss'],
})
export class VideostreamCardComponent implements OnInit {
  @Input() modal: ModalController;
  constructor() { }

  ngOnInit() {}

  /**
   * Dismisses the video playback modal.
   */
  async dismissModal() {
    await this.modal.dismiss()
  }

}
