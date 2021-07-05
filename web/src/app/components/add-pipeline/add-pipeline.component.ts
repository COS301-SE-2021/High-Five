import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {ModalController, PopoverController} from '@ionic/angular';
import {Pipeline} from '../../models/pipeline';
import {AddToolComponent} from '../add-tool/add-tool.component';

@Component({
  selector: 'app-add-pipeline',
  templateUrl: './add-pipeline.component.html',
  styleUrls: ['./add-pipeline.component.scss'],
})

/**
 * This class serves as a way to add pipelines
 */
export class AddPipelineComponent implements OnInit {
  @Input() availableTools: string[];
  pipeline: Pipeline = {};
  tools: string[] = [];

  constructor(private modalController: ModalController, private popoverController: PopoverController) {
    this.tools.sort((a, b) => a.localeCompare(b));
  }

  async dismiss() {
    if (this.pipeline && this.tools.length > 0) {
      this.pipeline.tools = this.tools;
    } else {
      this.pipeline = {};
    }
    await this.modalController.dismiss({
      dismissed: true,
      pipeline: this.pipeline
    });
  }

  async presentAddToolPopover(ev: any) {
    const selectedTools: string[] = [];
    const addToolPopover = await this.popoverController.create({
      component: AddToolComponent,
      event: ev,
      translucent: true,
      componentProps: {
        tools: selectedTools,
        availableTools: this.availableTools
      }
    });
    await addToolPopover.present();
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
