import {Component, OnInit} from '@angular/core';
import {ModalController, PopoverController} from '@ionic/angular';
import {AddItemComponent} from '../add-item/add-item.component';
import {PipelineService} from "../../services/pipeline/pipeline.service";

@Component({
  selector: 'app-add-pipeline',
  templateUrl: './add-pipeline.component.html',
  styleUrls: ['./add-pipeline.component.scss'],
})

/**
 * This class serves as a way to add pipelines
 */
export class AddPipelineComponent implements OnInit {
  pipelineName : string;
  tools: string[] = [];

  constructor(private modalController: ModalController, private popoverController: PopoverController , public pipelineService: PipelineService) {
    // Nothing added here
  }

  async dismiss() {
    await this.pipelineService.addPipeline(this.pipelineName,this.tools);
    await this.modalController.dismiss();
  }

  async presentAddToolPopover(ev: any) {
    const addToolPopover = await this.popoverController.create({
      component: AddItemComponent,
      event: ev,
      translucent: true,
      componentProps: {
        availableItems: this.pipelineService.tools.filter(tool => !this.tools.includes(tool))
      }
    });
    await addToolPopover.present();
    await addToolPopover.onDidDismiss().then(data =>{
      this.tools = this.tools.concat(data.data.items);
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
