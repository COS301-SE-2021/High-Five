import {Component, Input, OnInit, Optional} from '@angular/core';
import {ToolsetConstants} from '../../../constants/toolset-constants';
import {PipelineService} from '../../services/pipeline/pipeline.service';
import {PipelinesService} from '../../apis/pipelines.service';
import {Pipeline} from '../../models/pipeline';
import {CreatePipelineRequest} from '../../models/createPipelineRequest';
import {LoadingController} from '@ionic/angular';
import {DeletePipelineRequest} from '../../models/deletePipelineRequest';


@Component({
  selector: 'app-pipeline',
  templateUrl: './pipeline.component.html',
  styleUrls: ['./pipeline.component.scss'],
})
export class PipelineComponent implements OnInit {
  public pipelines: Pipeline[];
  constructor(public constants: ToolsetConstants, private pipelinesService: PipelinesService, private loadingController: LoadingController) {
    this.getAllPipelines();
  }

  ngOnInit() {}


  /**
   * A
   *
   * @param index is the index of the pipeline in the array
   */
  async deletePipeline(index: number){
      const loading = await this.loadingController.create({
        spinner: 'circles',
        animated:true,
      });
      await loading.present();
      const id: string= this.pipelines[index].id;
      const deletePipelineRequest: DeletePipelineRequest={
        pipelineId: id,
      };
      try{
        const res = this.pipelinesService.deletePipeline(deletePipelineRequest).subscribe(response =>{
          console.log(response);
          loading.dismiss();
          this.getAllPipelines();
        });
      }catch (e) {
        await loading.dismiss();
      }
  }

  async edit(){

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
