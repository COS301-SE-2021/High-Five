import {Component, OnInit} from '@angular/core';
import {ModalController, PopoverController, ToastController} from '@ionic/angular';
import {AddItemComponent} from '../add-item/add-item.component';
import {PipelineService} from '../../services/pipeline/pipeline.service';
import {Pipeline} from '../../models/pipeline';
import {UserToolsService} from '../../services/user-tools/user-tools.service';

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
              public pipelineService: PipelineService, private toastController: ToastController,
              private userToolsService: UserToolsService) {
    // Nothing added here
  }

  public async dismiss() {
    const newArr = this.pipelineService.pipelines.map((pipeline: Pipeline) => pipeline.name);
    if(this.pipelineName.replace(/\s/g, '').length <= 0){
      await this.toastController.create({
        message: 'Pipelines may not have blank names, please choose another name',
        duration: 2000,
        translucent: true,
        position: 'bottom'
      }).then((toast) => {
        toast.present();
      });
      return;
    }
    if (newArr.filter((value) => value === this.pipelineName).length > 0 ) {
      await this.toastController.create({
        message: 'Pipelines may not have duplicate names, please choose another name',
        duration: 2000,
        translucent: true,
        position: 'bottom'
      }).then((toast) => {
        toast.present();
      });
    } else {
      if (this.tools.length > 0) {
        if (this.userToolsService.drawingToolCount([this.tools[this.tools.length - 1]]) > 0) {
          await this.pipelineService.addPipeline(this.pipelineName, this.tools);
          await this.modalController.dismiss();
        } else {
          await this.toastController.create({
            message: `A pipeline's last tool must be a drawing tool`,
            duration: 2000,
            translucent: true,
            position: 'bottom'
          }).then((toast) => {
            toast.present();
          });
        }
      } else {
        await this.toastController.create({
          message: `A pipeline must have at least one tool`,
          duration: 2000,
          translucent: true,
          position: 'bottom'
        }).then((toast) => {
          toast.present();
        });
      }


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
        // eslint-disable-next-line max-len
        availableItems: this.userToolsService.userTools.filter(t => t.isApproved).map(t => t.toolName).filter(tool => !this.tools.includes(tool))
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

