import {Injectable} from '@angular/core';
import {PipelineModel} from '../../models/pipeline.model';
import {ToolsetConstants} from '../../../constants/toolset-constants';
import {PipelinesService} from '../../apis/pipelines.service';
import {BehaviorSubject, Observable} from 'rxjs';
import {distinctUntilChanged} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class PipelineService {
  private addedNew = new BehaviorSubject(false);
  private pipelines: PipelineModel[];
  constructor(private pipelineConstants: ToolsetConstants, private pipelinesService: PipelinesService) {
    this.pipelines=[];
  }

  public addPipelineModel(pipeline: PipelineModel){
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
  addedNewPipelineWatch(): Observable<boolean>{
    return this.addedNew.asObservable().pipe(distinctUntilChanged());
  }

  setNewPipelineAdded(value: boolean){
    this.addedNew.next(value);
  }


}
