import {Component, Input, OnInit} from '@angular/core';
import {LoadingController, ModalController, ToastController} from '@ionic/angular';
import {Pipeline} from '../../models/pipeline';
import {ToolsetConstants} from '../../../constants/toolset-constants';
import {PipelinesService} from '../../apis/pipelines.service';
import {RemoveToolsRequest} from '../../models/removeToolsRequest';
import {AddToolsRequest} from '../../models/addToolsRequest';

@Component({
  selector: 'app-edit-pipeline',
  templateUrl: './edit-pipeline.component.html',
  styleUrls: ['./edit-pipeline.component.scss'],
})
export class EditPipelineComponent implements OnInit {
  /**
   * The below inputs are all pased in from the pipeline component's function which opens the modal
   */
  @Input() modalController: ModalController;
  @Input() pipeline: Pipeline;
  @Input() loadingController: LoadingController;
  public selectedTools: boolean[];
  public tools: string[];

  /**
   *
   * @param toolsetConstants constants injected that represents all the tools and constants used by the pipeline and tools
   * @param pipelinesService the services which OpenAPI generated, which we will use to send requests
   * @param toastController toast controller that will enable us to display unobtrusive informative messages
   */
  constructor(private toolsetConstants: ToolsetConstants, private pipelinesService: PipelinesService,
              private toastController: ToastController) {
    this.tools = this.toolsetConstants.labels.tools;
    this.selectedTools = new Array<boolean>(this.tools.length);
  }

  ngOnInit() {
  }


  /**
   * This function closes the modal without performing any function or updating the pipeline's values, all changes will
   * be discarded when this function is called (if there were any changes made in the frontend to the tools in the
   * pipeline)
   */
  dismiss() {
    this.modalController.dismiss({
      dismissed: true
    });
  }

  /**
   * This function wil make requests using the models and services generated by OpenAPI to send requests to the
   * backend to add and/or remove tools from the pipeline
   */
  async applyChanges() {
    await this.modalController.dismiss({
      dismissed: true
    });
    const loading = await this.loadingController.create({
      spinner: 'circles',
      animated: true,
    });


    await loading.present();
    const toast = await this.toastController.create(
      {
        message: 'Successfully edited pipeline',
        duration: 2000
      }
    );
    toast.translucent = true; // Will only work on IOS

    const newTools: string[] = []; // Tools that need to be added that are not already selected
    const removeTools: string[] = []; // Tools that need to be removed
    for (let i = 0; i < this.tools.length; i++) {
      if (this.selectedTools[i]) {
        newTools.push(this.tools[i]);
      } else if(!this.selectedTools[i]){
        removeTools.push(this.tools[i]);
      }else{
        newTools.push(this.tools[i]);
      }
    }

    const removeToolsRequest: RemoveToolsRequest = {
      pipelineId: this.pipeline.id,
      tools: removeTools,
    };

    const addToolsRequest: AddToolsRequest = {
      pipelineId: this.pipeline.id,
      tools: newTools,
    };
    const res = this.pipelinesService.removeTools(removeToolsRequest).subscribe(response => {
      const res2 = this.pipelinesService.addTools(addToolsRequest).subscribe(addResponse => {
        this.pipeline.tools = newTools;
        loading.dismiss();
        toast.present();
      });
    });
  }
}


