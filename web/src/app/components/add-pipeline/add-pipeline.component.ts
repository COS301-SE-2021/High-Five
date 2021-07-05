import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {ModalController} from '@ionic/angular';
import {Pipeline} from '../../models/pipeline';

@Component({
  selector: 'app-add-pipeline',
  templateUrl: './add-pipeline.component.html',
  styleUrls: ['./add-pipeline.component.scss'],
})

/**
 * This class serves as a way to add pipelines
 */
export class AddPipelineComponent implements OnInit {
  pipeline: Pipeline = {};
  tools: string[] = [];

  constructor(private modalController: ModalController) {
    for (let i = 0; i < 20; i++) {
      this.tools.push('ASD' + i);
    }
    this.tools.sort((a, b) => a.localeCompare(b));
  }

  /**
   * This function closes the modal without performing any function or updating the pipeline's values, all changes will
   * be discarded when this function is called (if there were any changes made in the frontend to the tools in the
   * pipeline)
   */
  async dismiss() {
    await this.modalController.dismiss({
      dismissed: true,
      pipeline: this.pipeline
    });
  }


  ngOnInit() {
  }

  async dismissWithoutSaving() {
    await this.modalController.dismiss({
      dismissed: true,
    });
  }

  removeTool(tool: string) {
    this.tools = this.tools.filter(t => t !== tool);
  }
}
