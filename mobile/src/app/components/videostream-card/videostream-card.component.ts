import {Component, Input, OnInit} from '@angular/core';
import {ModalController} from '@ionic/angular';
import {Endpoints} from '../../../constants/endpoints';
@Component({
  selector: 'app-videostream-card',
  templateUrl: './videostream-card.component.html',
  styleUrls: ['./videostream-card.component.scss'],
})
export class VideostreamCardComponent implements OnInit {
  @Input() modal: ModalController;
  @Input() vidId: string;
  constructor(public readonly endpoints: Endpoints) { }

  ngOnInit() {}

  /**
   * Dismisses the video playback modal.
   */
  async dismissModal() {
    await this.modal.dismiss();
  }

}
