import {Component, Input, OnInit} from '@angular/core';
import {ModalController} from '@ionic/angular';
import {Pipeline} from '../../models/pipeline';

@Component({
  selector: 'app-edit-pipeline',
  templateUrl: './edit-pipeline.component.html',
  styleUrls: ['./edit-pipeline.component.scss'],
})
export class EditPipelineComponent implements OnInit {
  @Input() modalController: ModalController;
  @Input() pipeline: Pipeline;
  @Input() tools: string[];
  constructor() { }

  ngOnInit() {}

  dismiss(){
    this.modalController.dismiss({
      dismissed : true
    });
  }

}
