import {Injectable} from '@angular/core';
import {BehaviorSubject} from 'rxjs';
import {MediaStorageService} from '../../apis/mediaStorage.service';
import {AnalyzedVideoMetaData} from '../../models/analyzedVideoMetaData';
import {AnalysisService} from '../../apis/analysis.service';

@Injectable({
  providedIn: 'root'
})
export class AnalyzedVideosService {


  private readonly _analyzeVideos = new BehaviorSubject<AnalyzedVideoMetaData[]>([]);
  // eslint-disable-next-line @typescript-eslint/member-ordering,no-underscore-dangle
  readonly analyzedVideos$ = this._analyzeVideos.asObservable();

  constructor(private mediaStorageService: MediaStorageService, private analysisService: AnalysisService) {
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

  public async analyzeVideo(mediaId: string, pipelineId: string, mediaType: string = 'video') {
    await this.analysisService.analyzeMedia({
      mediaId,
      pipelineId,
      mediaType
    }).toPromise();
    await this.fetchAll();
  }

  public async fetchAll() {
    await this.mediaStorageService.getAnalyzedVideos().subscribe((res) => {
      this.analyzeVideos = res.videos;
    });
  }
}
