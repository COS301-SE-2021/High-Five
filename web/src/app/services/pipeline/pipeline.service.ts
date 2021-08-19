import {Injectable} from '@angular/core';
import {BehaviorSubject} from 'rxjs';
import {Pipeline} from '../../models/pipeline';
import {PipelinesService} from '../../apis/pipelines.service';


@Injectable({
  providedIn: 'root'
})
export class PipelineService {

  private readonly _pipelines = new BehaviorSubject<Pipeline[]>([]);
  // eslint-disable-next-line @typescript-eslint/member-ordering,no-underscore-dangle
  readonly pipelines$ = this._pipelines.asObservable();
  private readonly _tools = new BehaviorSubject<string[]>([]);
  // eslint-disable-next-line @typescript-eslint/member-ordering,no-underscore-dangle
  readonly tools$ = this._tools.asObservable();

  constructor(private pipelinesService: PipelinesService) {
    this.fetchAllPipelines();
    this.fetchAllTools();
  }

  get pipelines(): Pipeline[] {
    // eslint-disable-next-line no-underscore-dangle
    return this._pipelines.getValue();
  }

  set pipelines(val: Pipeline[]) {
    // eslint-disable-next-line no-underscore-dangle
    this._pipelines.next(val);
  }

  get tools(): string[] {
    // eslint-disable-next-line no-underscore-dangle
    return this._tools.getValue();
  }

  set tools(val: string[]) {
    // eslint-disable-next-line no-underscore-dangle
    this._tools.next(val);
  }

  async addPipeline(name: string, tools: string[]) {
    try {
      await this.pipelinesService.createPipeline({pipeline: {name, tools}}).toPromise();
      await this.fetchAllPipelines();
    } catch (e) {
      console.log(e);
    }
  }

  public async removePipeline(pipelineId: string, serverRemove: boolean = true) {
    const pipeline = this.pipelines.find(p => p.id === pipelineId);
    this.pipelines = this.pipelines.filter(p => p.id !== pipelineId);
    if (serverRemove) {
      try {
        await this.pipelinesService.deletePipeline({pipelineId}).toPromise();
      } catch (e) {
        console.error(e);
        this.pipelines = [...this.pipelines, pipeline];
      }
    }
  }

  public async addTool(id: string, tools: string[]) {
    const pipeline = this.pipelines.find(p => p.id === id);
    console.log('Old ');
    if (pipeline) {
      const index = this.pipelines.indexOf(pipeline);

      this.pipelines[index] = {
        ...pipeline,
        tools: pipeline.tools.concat(tools)
      };

      this.pipelines = [...this.pipelines];

      try {
        await this.pipelinesService.addTools({pipelineId: id, tools}).toPromise();
      } catch (e) {
        console.error(e);
        this.pipelines[index] = {
          ...pipeline,
          tools: pipeline.tools
        };
      }
    }
  }

  public async removeTool(id: string, tools: string[]) {
    const pipeline = this.pipelines.find(p => p.id === id);
    if (pipeline) {
      const index = this.pipelines.indexOf(pipeline);

      this.pipelines[index] = {
        ...pipeline,
        tools: pipeline.tools.filter((e) => tools.some((f) => e !== f))
      };

      this.pipelines = [...this.pipelines];

      try {
        await this.pipelinesService.removeTools({pipelineId: id, tools}).toPromise();
      } catch (e) {
        console.error(e);
        this.pipelines[index] = {
          ...pipeline,
          tools: pipeline.tools
        };
      }
    }
  }

  public async fetchAllPipelines() {
    await this.pipelinesService.getPipelines().subscribe((res) => {
      this.pipelines = res.pipelines;
    });
  }

  public async fetchAllTools() {
    this.tools = await this.pipelinesService.getAllTools().toPromise();
  }
}
