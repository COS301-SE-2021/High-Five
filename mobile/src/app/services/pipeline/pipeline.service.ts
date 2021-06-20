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
  constructor() {}

  /**
   * This function will emit act as an event emitter when the value of addedNew changes, only when the value gets
   * changed to a different one that it currently is.
   */
  addedNewPipelineWatch(): Observable<boolean>{
    return this.addedNew.asObservable().pipe(distinctUntilChanged());
  }

  /**
   * This function allows us to update the value of the
   *
   * @param value the value that we would like the addedNew value to take on as a boolean (true or false)
   */
  setNewPipelineAdded(value: boolean){
    this.addedNew.next(value);
  }


}
