import {Injectable} from '@angular/core';
import {BehaviorSubject} from 'rxjs';
import {MediaStorageService} from '../../apis/mediaStorage.service';
import {AnalyzedVideoMetaData} from '../../models/analyzedVideoMetaData';
import {AnalysisService} from '../../apis/analysis.service';
import {WebsocketService} from '../websocket/websocket.service';
import {SnotifyService} from 'ng-snotify';

@Injectable({
  providedIn: 'root'
})
export class AnalyzedVideosService {


  private readonly _analyzeVideos = new BehaviorSubject<AnalyzedVideoMetaData[]>([]);
  // eslint-disable-next-line @typescript-eslint/member-ordering,no-underscore-dangle
  readonly analyzedVideos$ = this._analyzeVideos.asObservable();

  constructor(private mediaStorageService: MediaStorageService, private analysisService: AnalysisService,
              private websocketService: WebsocketService, private snotifyService: SnotifyService) {
    this.fetchAll();
  }

  get analyzeVideos(): AnalyzedVideoMetaData[] {
    // eslint-disable-next-line no-underscore-dangle
    return this._analyzeVideos.getValue();
  }

  set analyzeVideos(val: AnalyzedVideoMetaData[]) {
    // eslint-disable-next-line no-underscore-dangle
    this._analyzeVideos.next(val);
  }


  /**
   * Function will send a request to analyze a media with the specified mediaId, pipelineId and media type
   *
   * @param mediaId the id of the media which to analyze (video or image)
   * @param pipelineId the id of the pipeline with which to analyze the media
   * @param mediaType the media type, video or image
   */
  public async analyzeVideo(mediaId: string, pipelineId: string, mediaType: string = 'video') {
    this.websocketService.sendMessage({
      // eslint-disable-next-line @typescript-eslint/naming-convention
      Request: 'AnalyzeVideo',
      // eslint-disable-next-line @typescript-eslint/naming-convention
      Body: {
        imageId: mediaId,
        piplineId: pipelineId,
      },
    });
    await this.fetchAll();
  }

  public async fetchAll() {
    await this.mediaStorageService.getAnalyzedVideos().subscribe((res) => {
      this.analyzeVideos = res.videos;
    });
  }

  public async deleteAnalyzedVideo(videoId: string, serverRemove: boolean = true) {
    const analyzedVideo = this.analyzeVideos.find(v => v.id === videoId);
    this.analyzeVideos = this.analyzeVideos.filter(v => v.id !== videoId);

    if (serverRemove) {
      try {
        this.mediaStorageService.deleteAnalyzedVideo({id: videoId}, 'response').subscribe((res) => {
          if (res.ok) {
            this.snotifyService.success('Successfully removed analyzed Video', 'Analyzed Video Removal');
          } else {
            this.snotifyService.error('Error occurred while removing Video, please contact an admin', 'Analyzed Video Removal');
            this.analyzeVideos = [...this.analyzeVideos, analyzedVideo];
          }
        });
      } catch (e) {
        this.snotifyService.error('Error occurred while removing analyzed video, please contact an admin', 'Analyzed Video Removal');
        console.error(e);
        this.analyzeVideos = [...this.analyzeVideos, analyzedVideo];
      }
    }
  }
}
