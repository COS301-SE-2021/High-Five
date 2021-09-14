import {Injectable} from '@angular/core';
import {BehaviorSubject} from 'rxjs';
import {Pipeline} from '../../models/pipeline';
import {PipelinesService} from '../../apis/pipelines.service';
import {SnotifyService} from 'ng-snotify';

@Injectable({
  providedIn: 'root'
})
export class UserPreferencesService {

  private readonly _mediaFilter = new BehaviorSubject<string>('all');
  // eslint-disable-next-line @typescript-eslint/member-ordering,no-underscore-dangle
  readonly mediaFilter$ = this._mediaFilter.asObservable();

  private readonly _liveAnalysisPipeline = new BehaviorSubject<Pipeline>(null);
  // eslint-disable-next-line @typescript-eslint/member-ordering,no-underscore-dangle
  readonly liveAnalysisPipeline$ = this._liveAnalysisPipeline.asObservable();

  constructor(private apiPipelineService: PipelinesService, private snotifyService: SnotifyService) {
    this.mediaFilter = 'all';
    this.setInitialLiveAnalysisPipeline();
  }

  get mediaFilter(): string {
    // eslint-disable-next-line no-underscore-dangle
    return this._mediaFilter.getValue();
  }

  set mediaFilter(val: string) {
    // eslint-disable-next-line no-underscore-dangle
    this._mediaFilter.next(val);
  }

  set liveAnalysisPipeline(pipeline: Pipeline) {
    // eslint-disable-next-line no-underscore-dangle
    this._liveAnalysisPipeline.next(pipeline);
    this.apiPipelineService.setLivePipeline({pipelineId: pipeline.id}).subscribe(() => {
      this.snotifyService.success(`Successfully updated live analysis pipeline to : ` + pipeline.name, 'Live Analysis Pipeline');
    });
  }

  get liveAnalysisPipeline(): Pipeline {
    // eslint-disable-next-line no-underscore-dangle
    return this._liveAnalysisPipeline.value;
  }


  private async setInitialLiveAnalysisPipeline() {
    this.apiPipelineService.getLivePipeline().subscribe((res) => {
      this.liveAnalysisPipeline = res;
    });
  }

}
