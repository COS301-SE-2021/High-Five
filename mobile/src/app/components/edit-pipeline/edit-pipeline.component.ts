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
  @Input() modalController: ModalController;
  @Input() pipeline: Pipeline;
  @Input() loadingController: LoadingController;
  public selectedTools: boolean[];
  public tools: string[];

  constructor(private toolsetConstants: ToolsetConstants, private pipelinesService: PipelinesService,
              private toastController: ToastController) {
    this.tools = this.toolsetConstants.labels.tools;
    this.selectedTools = new Array<boolean>(this.tools.length);
  }

  ngOnInit() {
  }

  dismiss() {
    this.modalController.dismiss({
      dismissed: true
    });
  }

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


