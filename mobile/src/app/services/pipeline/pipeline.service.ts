import { Injectable } from '@angular/core';
import {PipelineModel} from '../../models/pipeline.model';

@Injectable({
  providedIn: 'root'
})
export class PipelineService {

  private pipelines: PipelineModel[];
  constructor() {
    this.pipelines=[];
  }

  public addPipelineModel(pipeline: PipelineModel){
    this.pipelines.push(pipeline);
  }

  public addPipeline(selectedTools: boolean[], pipelineName: string){
    const temp: boolean[]=new Array<boolean>(selectedTools.length);
    for (let i =0; i < selectedTools.length; i++){
      temp[i] = selectedTools[i];
    }
    this.pipelines.push(new PipelineModel(pipelineName,temp));
  }

  public removePipeline(id: number){
    for (let i = 0; i < this.pipelines.length; i++) {
      if (this.pipelines[i].pipelineId===id){
        delete this.pipelines[i];
        break;
      }
    }
  }

  get allPipelines(): PipelineModel[]{
    return this.pipelines;
  }

  set allPipelines(pipelineModels: PipelineModel[]){
    this.pipelines=pipelineModels;
  }

}
