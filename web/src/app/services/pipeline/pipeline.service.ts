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

  /**
   * Function adds a pipeline by sending a request using the OpenAPI generate PipelinesService
   *
   * @param name a string representing the name of the pipeline
   * @param tools an array of string representing the tools of the pipeline
   */
  async addPipeline(name: string, tools: string[]) {
    try {
      await this.pipelinesService.createPipeline({pipeline: {name, tools}}, 'response').subscribe((res) => {
        if (res.ok) {
          // TODO : Notification here
          this.fetchAllPipelines();
        } else {
          // TODO : Notification here
        }
      });
    } catch (e) {
      // TODO : Notification here
      console.log(e);
    }
  }

  /**
   *
   * @param pipelineId the id of the pipeline which to remove (string)
   * @param serverRemove boolean, if true  will send a request to the backend to remove the pipeline, otherwise
   * the pipeline will only be removed locally, by default this parameter is set to true
   */
  public async removePipeline(pipelineId: string, serverRemove: boolean = true) {
    const pipeline = this.pipelines.find(p => p.id === pipelineId);
    this.pipelines = this.pipelines.filter(p => p.id !== pipelineId);
    if (serverRemove) {
      try {
        await this.pipelinesService.deletePipeline({pipelineId}, 'response').subscribe((res) => {
          if (res.ok) {
            // TODO : Notification here
          } else {
            // TODO : Notification here
            this.pipelines = [...this.pipelines, pipeline];
          }
        });
      } catch (e) {
        // TODO : Notification here
        console.error(e);
        this.pipelines = [...this.pipelines, pipeline];
      }
    }
  }

  public async addTool(id: string, tools: string[]) {
    const pipeline = this.pipelines.find(p => p.id === id);
    if (pipeline) {
      const index = this.pipelines.indexOf(pipeline);
      this.pipelines[index] = {
        ...pipeline,
        tools: pipeline.tools.concat(tools)
      };

      this.pipelines = [...this.pipelines];

      try {
        await this.pipelinesService.addTools({pipelineId: id, tools}, 'response').subscribe((res) => {
          if (res.ok) {
            // TODO : Notification here
          } else {
            // TODO : Notification here
            this.pipelines[index] = {
              ...pipeline,
              tools: pipeline.tools
            };
          }
        });
      } catch (e) {
        // TODO : Notification here
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
        await this.pipelinesService.removeTools({pipelineId: id, tools}, 'response').subscribe((res) => {
          if (res.ok) {
            // TODO : Notification here
          } else {
            // TODO : Notification here
            this.pipelines[index] = {
              ...pipeline,
              tools: pipeline.tools
            };
          }
        });
      } catch (e) {
        // TODO : Notification here
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
