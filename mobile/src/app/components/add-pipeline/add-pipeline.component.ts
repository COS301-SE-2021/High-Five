import {Component, OnInit} from '@angular/core';
import {ToolsetConstants} from '../../../constants/toolset-constants';
import {PipelinesService} from '../../apis/pipelines.service';
import {Pipeline} from '../../models/pipeline';
import {CreatePipelineRequest} from '../../models/createPipelineRequest';
import {LoadingController, ToastController} from '@ionic/angular';
import {PipelineComponent} from '../pipeline/pipeline.component';

@Component({
  selector: 'app-add-pipeline',
  templateUrl: './add-pipeline.component.html',
  styleUrls: ['./add-pipeline.component.scss'],
  providers: [PipelineComponent]
})
export class AddPipelineComponent implements OnInit {
  selectedTools: boolean[];
  pipelineName: string;
  constructor(public constants: ToolsetConstants, public pipelinesService: PipelinesService, private loadingController: LoadingController, private pipelinesComp: PipelineComponent, private toastController: ToastController) {
    this.selectedTools = new Array<boolean>(this.constants.labels.tools.length);
  }

  async addPipeline(){
    const loading = await this.loadingController.create({
      spinner: 'circles',
      animated:true,
    });
    await loading.present();
    const toast = await  this.toastController.create(
      {
        message: 'Pipeline successfully created',
        duration: 2000
      }
    );
    const temp: string[] = [];
    let allEmpty = true;
    for (let i = 0; i < this.selectedTools.length; i++) {
      if(this.selectedTools[i]){
        temp.push(this.constants.labels.tools[i]);
        allEmpty= false;
      }
    }
    if(allEmpty){
      //todo create alert to user that the pipeline needs at least one pipeline
      await loading.dismiss();
      return;
    }

    const newPipeline: Pipeline ={
      name:this.pipelineName,
      tools: temp,
    };

    const newPipelineRequest: CreatePipelineRequest = {
      pipeline: newPipeline,
    };

    const res = this.pipelinesService.createPipeline(newPipelineRequest).subscribe(
      response =>{
        if (!response.success){
          toast.message = 'Error occurred whilst creating pipeline';
        }
        loading.dismiss();
        toast.present();

      }
    );
  }
  ngOnInit() {}

}
