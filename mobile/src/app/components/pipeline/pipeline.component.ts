import {Component, Input, OnInit, Optional} from '@angular/core';
import {ToolsetConstants} from '../../../constants/toolset-constants';
import {PipelineService} from '../../services/pipeline/pipeline.service';
import {PipelinesService} from '../../apis/pipelines.service';
import {Pipeline} from '../../models/pipeline';
import {CreatePipelineRequest} from '../../models/createPipelineRequest';
import {LoadingController, ModalController, ToastController} from '@ionic/angular';
import {DeletePipelineRequest} from '../../models/deletePipelineRequest';
import {EditPipelineComponent} from '../edit-pipeline/edit-pipeline.component';


@Component({
  selector: 'app-pipeline',
  templateUrl: './pipeline.component.html',
  styleUrls: ['./pipeline.component.scss'],
})
export class PipelineComponent implements OnInit {
  public pipelines: Pipeline[];
  constructor(public constants: ToolsetConstants, private pipelinesService: PipelinesService,
              private loadingController: LoadingController, private modalController: ModalController,
              private toastController: ToastController) {
    this.getAllPipelines();
  }

  ngOnInit() {}


  /**
   * A function that will delete a pipeline based off of its id that it gets by getting
   * the index of the pipeline from the local pipelines array
   *
   * @param index is the index of the pipeline in the array
   */
  async deletePipeline(index: number){
      const loading = await this.loadingController.create({
        spinner: 'circles',
        animated:true,
      });
      await loading.present();
    const toast = await  this.toastController.create(
      {
        message: 'Pipeline successfully deleted',
        duration: 2000
      }
    );
      const id: string= this.pipelines[index].id;
      const deletePipelineRequest: DeletePipelineRequest={
        pipelineId: id,
      };
      try{
        const res = this.pipelinesService.deletePipeline(deletePipelineRequest).subscribe(response =>{
          loading.dismiss();
          toast.present();
          this.getAllPipelines();
        });
      }catch (e) {
        toast.message= 'Error occured while deleting pipeline';
        await loading.dismiss();
        await toast.present();
      }
  }


  async edit(){

  }

  async startEditProcess(i){
    const modal = await this.modalController.create({
      component : EditPipelineComponent,
      cssClass : 'editPipeline',
      componentProps:{
        modalController : this.modalController,
        tools : this.constants.labels.tools,
        pipeline : this.pipelines[i],
        loadingController : this.loadingController,
      }
    });
    modal.style.backgroundColor = 'rgba(0,0,0,0.85)';
    return await  modal.present();
  }


  /**
   * A function that sends a request to get all the created pipelines
   */
  async getAllPipelines(){
    const x = this.pipelinesService.getPipelines().subscribe(
      response => {
        this.pipelines= response.pipelines;
      }
    );
  }
}
