import {Injectable} from '@angular/core';
import {PipelineModel} from '../../models/pipeline.model';
import {ToolsetConstants} from '../../../constants/toolset-constants';

@Injectable({
  providedIn: 'root'
})
export class PipelineService {

  private pipelines: PipelineModel[];
  constructor(private pipelineConstants: ToolsetConstants) {
    this.pipelines=[];
  }

  public addPipelineModel(pipeline: PipelineModel){
    this.pipelines.push(pipeline);
  }

  async addPipeline(selectedTools: boolean[], pipelineName: string){
    const temp: boolean[]=new Array<boolean>(selectedTools.length);
    const toolNames: string[] = [];
    for (let i =0; i < selectedTools.length; i++){
      if (selectedTools[i]){
        temp[i] = true;
        toolNames.push(this.pipelineConstants.labels.tools[i]);
      }else{
        temp[i] = false;
      }
    }


    this.pipelines.push(new PipelineModel(pipelineName,temp));
    // const newPipeline: NewPipeline={
    //   name: pipelineName,
    //   tools : toolNames,
    // };

    // const createPipelineRequest: CreatePipelineRequest={
    //   pipeline : newPipeline
    // };
    // const res = this.pipelinesService.createPipeline(createPipelineRequest).subscribe(
    //   response =>{
    //     console.log(response.message);
    //   }
    // );
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
