import {Component, OnInit} from '@angular/core';
import {ScreenSizeServiceService} from '../../services/screen-size-service.service';
import {Pipeline} from '../../models/pipeline';
import {PipelinesService} from '../../apis/pipelines.service';
import {ModalController, ToastController} from '@ionic/angular';
import {EditPipelineComponent} from '../../components/edit-pipeline/edit-pipeline.component';
import {AddPipelineComponent} from '../../components/add-pipeline/add-pipeline.component';
import {NewPipeline} from '../../models/newPipeline';
import {CreatePipelineRequest} from '../../models/createPipelineRequest';

@Component({
  selector: 'app-analytics',
  templateUrl: './analytics.page.html',
  styleUrls: ['./analytics.page.scss'],
})
export class AnalyticsPage implements OnInit {

  public isDesktop: boolean;
  public pipelines: Pipeline[] = [];
  public availableTools: string[] = [];

  constructor(private screenSizeService: ScreenSizeServiceService, private pipelinesService: PipelinesService,
              private toastController: ToastController, private modalController: ModalController) {
    this.screenSizeService.isDesktopView().subscribe(isDesktop => {
      this.isDesktop = isDesktop;
    });
  }

  /**
   * Deletes a pipeline from the local pipelines array and sends a qpi request to remove the pipeline
   *
   * @param id the id of the pipeline that should be removed
   */
  deletePipeline(id: string) {
    this.pipelines = this.pipelines.filter(pipeline => pipeline.id !== id);
    const toast = this.toastController.create({
      message: 'Successfully deleted pipeline',
      duration: 2000,
      translucent: true
    }).then(m => m.present());
  }

  removeTool(pipeline: Pipeline) {
    const toast = this.toastController.create({
      header: 'Success!',
      message: 'removed tool from ' + pipeline.name,
      duration: 2000,
      translucent: true,
      position: 'bottom'
    }).then(m => m.present());
  }

  ngOnInit() {
    let updatedTools = false;
    let updatedPipelines = false;
    this.updateAvailableTools().then(res => {
      updatedTools = res;
    });
    this.updatePipelines().then(res => {
      updatedPipelines = res;
    });
  }


  async openAddPipelineModal() {

    const modal = await this.modalController.create({
      component: AddPipelineComponent,
      cssClass: 'add-pipeline-modal',
      showBackdrop: true,
      animated: true,
      backdropDismiss: false,
      componentProps: {
        availableTools: this.availableTools
      }
    });
    modal.onWillDismiss().then(data => {
      if (data.data.pipeline) {
        if (data.data.pipeline.name && data.data.pipeline.tools) { //Data validation
          const newPipeline: NewPipeline = {
            name: data.data.pipeline.name,
            tools: data.data.pipeline.tools
          };
          const createPipelineRequest: CreatePipelineRequest = {
            pipeline: newPipeline
          };
          this.pipelines.push(data.data.pipeline); // Optimistic update
          this.pipelines.sort((a, b) => a.name.localeCompare(b.name));
          this.pipelinesService.createPipeline(createPipelineRequest).subscribe(response => {
            /**
             * If the response receives no id , it can be assumed that the request to the backend server failed,
             * therefore we will undo the optimistic update
             */
            if (response.pipelineId == null) {
              this.pipelines = this.pipelines.filter(pipeline => pipeline.id !== data.data.pipeline.id);
            }
          });
        } else {
          const toast = this.toastController.create({
            message: 'All necessary data of the pipeline was not present, please try again',
            duration: 1000,
            translucent: true,
            position: 'bottom'
          }).then(m => m.present());
        }
      }
    });
    return modal.present();
  }

  private async updatePipelines(): Promise<boolean> {
    this.pipelinesService.getPipelines().subscribe(response => {
      this.pipelines = response.pipelines;
      this.pipelines.sort((a, b) => a.name.localeCompare(b.name));
      return true;
    });
    return false;
  }

  private async updateAvailableTools(): Promise<boolean> {
    this.pipelinesService.getAllTools().subscribe(response => {
      this.availableTools = response;
      this.pipelines.sort((a, b) => a.name.localeCompare(b.name));
      return true;
    });
    return false;
  }
}



