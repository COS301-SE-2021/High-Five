import {Component, Input, OnInit} from '@angular/core';
import {ModalController, PopoverController} from '@ionic/angular';
import {Pipeline} from '../../models/pipeline';
import {AddItemComponent} from '../add-item/add-item.component';

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
    this.pipeline= {};
  }

  async dismiss() {
    if (this.pipeline && this.tools.length > 0) {
      this.pipeline.tools = this.tools;
      this.pipeline.tools.sort((a, b) => a.localeCompare(b));
    } else {
      this.pipeline = {};
    }
    await this.modalController.dismiss({
      dismissed: true,
      pipeline: this.pipeline
    });
  }

  async presentAddToolPopover(ev: any) {
    const addToolPopover = await this.popoverController.create({
      component: AddItemComponent,
      event: ev,
      translucent: true,
      componentProps: {
        availableItems: this.availableTools.filter(tool => !this.tools.includes(tool)),
        title : 'Add Tools'
      }
    });
    await addToolPopover.present();
    await addToolPopover.onDidDismiss().then(data =>{
      if(data.data != undefined){
        if(data.data.items != undefined){
          console.log(this.tools);
          this.tools = this.tools.concat(data.data.items);
        }
      }
    });
  }

  ngOnInit() {
    //Nothing to add here yet
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
