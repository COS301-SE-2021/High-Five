import {Component, Input, OnInit} from '@angular/core';
import {ModalController} from '@ionic/angular';

@Component({
  selector: 'app-add-pipeline',
  templateUrl: './add-pipeline.component.html',
  styleUrls: ['./add-pipeline.component.scss'],
})
export class AddPipelineComponent implements OnInit {
  @Input() modal: ModalController;
  constructor() { }

  ngOnInit() {}
  async dismissModal() {
   await this.modal.dismiss();
  }
}
