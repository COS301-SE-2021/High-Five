import {Injectable} from '@angular/core';
import {BehaviorSubject} from "rxjs";
import {Pipeline} from "../../models/pipeline";
import {PipelinesService} from "../../apis/pipelines.service";


@Injectable({
  providedIn: 'root'
})
export class PipelineService {

  private readonly _pipelines = new BehaviorSubject<Pipeline[]>([])
  readonly pipelines$ = this._pipelines.asObservable();
  private readonly _tools = new BehaviorSubject<string[]>([])
  readonly tools$ = this._tools.asObservable();

  constructor(private pipelinesService: PipelinesService) {
    this.fetchAllPipelines();
    this.fetchAllTools();
  }

  get pipelines(): Pipeline[] {
    return this._pipelines.getValue();
  }

  set pipelines(val: Pipeline[]) {
    this._pipelines.next(val);
  }

  get tools(): string[] {
    return this._tools.getValue();
  }

  set tools(val: string[]) {
    this._tools.next(val);
  }

  async addPipeline(name: string, tools: string[]) {
    try {
      await this.pipelinesService.createPipeline({pipeline: {name: name, tools: tools}}).toPromise();
      this.fetchAllPipelines();
    } catch (e) {
      console.log(e);
    }
  }

  async removePipeline(pipelineId: string, serverRemove: boolean = true) {
    const pipeline = this.pipelines.find(p => p.id === pipelineId);
    this.pipelines = this.pipelines.filter(pipeline => pipeline.id !== pipelineId);
    if (serverRemove) {
      try {
        await this.pipelinesService.deletePipeline({pipelineId: pipelineId}).toPromise();
      } catch (e) {
        console.error(e);
        this.pipelines = [...this.pipelines, pipeline];
      }
    }
  }

  async addTool(id: string, tools: string[]) {
    const pipeline = this.pipelines.find(pipeline => pipeline.id === id);
    console.log("Old ")
    if (pipeline) {
      const index = this.pipelines.indexOf(pipeline);

      this.pipelines[index] = {
        ...pipeline,
        tools: pipeline.tools.concat(tools)
      }

      this.pipelines = [...this.pipelines];

      try {
        await this.pipelinesService.addTools({pipelineId: id,tools: tools}).toPromise();
      } catch (e) {
        console.error(e);
        this.pipelines[index] = {
          ...pipeline,
          tools: pipeline.tools
        }
      }
    }
  }

  async removeTool(id: string, tools: string[]) {
    const pipeline = this.pipelines.find(pipeline => pipeline.id === id);
    if (pipeline) {
      const index = this.pipelines.indexOf(pipeline);

      this.pipelines[index] = {
        ...pipeline,
        tools: pipeline.tools.filter((e) => {
          return tools.some((f) => {
            return e !== f;
          })
        })
      }

      this.pipelines = [...this.pipelines];

      try {
        await this.pipelinesService.removeTools({pipelineId: id,tools: tools}).toPromise();
      } catch (e) {
        console.error(e);
        this.pipelines[index] = {
          ...pipeline,
          tools: pipeline.tools
        }
      }
    }
  }

  async fetchAllPipelines() {
    await this.pipelinesService.getPipelines().subscribe((res) => {
      this.pipelines = res.pipelines;
    });
  }

  async fetchAllTools() {
    this.tools = await this.pipelinesService.getAllTools().toPromise();
  }
}
