import {Component, OnInit} from '@angular/core';
import {ModalController, PopoverController, ToastController} from '@ionic/angular';
import {AddItemComponent} from '../add-item/add-item.component';
import {PipelineService} from '../../services/pipeline/pipeline.service';
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
  pipelineName: string;
  tools: string[] = [];

  constructor(private modalController: ModalController, private popoverController: PopoverController,
              public pipelineService: PipelineService, private toastController: ToastController) {
    // Nothing added here
  }

  public async dismiss() {
    const newArr = this.pipelineService.pipelines.map((pipeline: Pipeline) => pipeline.name);
    if (newArr.filter((value) => value === this.pipelineName).length > 0) {
      await this.toastController.create({
        message: 'Pipelines may not have duplicate names, please choose another name',
        duration: 2000,
        translucent: true,
        position: 'bottom'
      }).then((toast) => {
        toast.present();
      });
      await this.modalController.dismiss();
    } else {
      await this.pipelineService.addPipeline(this.pipelineName, this.tools);
      await this.modalController.dismiss();
    }

  }


  /**
   * Shows a popover containing all the tools which are available to add, excluding the tools already present in the
   * tools array
   *
   * @param ev the event which activates the popover
   */
  public async presentAddToolPopover(ev: any) {
    const addToolPopover = await this.popoverController.create({
      component: AddItemComponent,
      event: ev,
      translucent: true,
      componentProps: {
        availableItems: this.pipelineService.tools.filter(tool => !this.tools.includes(tool))
      }
    });
    await addToolPopover.present();
    await addToolPopover.onDidDismiss().then(data => {
      this.tools = this.tools.concat(data.data.items);
    });
  }

  ngOnInit() {
    //Nothing to add here yet
  }

  /**
   * Dismisses the modal without sending any data back to the parent component
   */
  public async dismissWithoutSaving() {
    await this.modalController.dismiss({
      dismissed: true,
    });
  }

  /**
   * removes a tool from the array of tools
   *
   * @param tool a string representing which tool should be removed
   */
  public removeTool(tool: string) {
    this.tools = this.tools.filter(t => t !== tool);
  }
}
