import {Injectable} from '@angular/core';
import {BehaviorSubject} from 'rxjs';
import {SnotifyService} from 'ng-snotify';
import {LiveStream} from '../../models/liveStream';
import {LivestreamService} from '../../apis/livestream.service';
import {MsalService} from '@azure/msal-angular';

@Injectable({
  providedIn: 'root'
})
export class LiveStreamingService {
  public appName: string;
  private readonly _streams = new BehaviorSubject<LiveStream[]>([]);
  // eslint-disable-next-line @typescript-eslint/member-ordering,no-underscore-dangle
  readonly streams$ = this._streams.asObservable();

  constructor(private liveStreamService: LivestreamService, private snotifyService: SnotifyService, private msalService: MsalService) {
    this.fetchAll();
    this.appName = msalService.instance.getActiveAccount().localAccountId;
    this.appName = this.appName.replace(/-/g, '');
    console.log(this.appName);
  }

  get streams(): LiveStream[] {
    // eslint-disable-next-line no-underscore-dangle
    return this._streams.getValue();
  }

  set streams(val: LiveStream[]) {
    // eslint-disable-next-line no-underscore-dangle
    this._streams.next(val);
  }


  public async getNewTokenForStreamId(streamId: string) {

  }

  /**
   * Uploads a video
   *
   * @param liveStream
   */
  public async addStream(liveStream: LiveStream) {
    this.streams = this.streams.concat(liveStream);
  }


  public async removeStreams(streamId: string) {
    this.streams = this.streams.filter(s => s.streamId !== streamId);
  }

  /**
   * Makes a request to retrieve all images
   */
  public async fetchAll() {
    this.liveStreamService.returnAllLiveStreams().subscribe((res) => {
      for (const stream of JSON.parse(res.message)) {
        this.streams = this.streams.concat({streamId: stream.name});
      }
    });
  }
}
