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

  public addPipeline(pipeline: PipelineModel){
    this.pipelines.push(pipeline);
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
