import { Injectable } from '@angular/core';
import {BehaviorSubject} from "rxjs";
import {MediaStorageService} from "../../apis/mediaStorage.service";
import {AnalyzedVideoMetaData} from "../../models/analyzedVideoMetaData";
import {AnalysisService} from "../../apis/analysis.service";

@Injectable({
  providedIn: 'root'
})
export class AnalyzedVideosService {


  private readonly _analyzeVideos = new BehaviorSubject<AnalyzedVideoMetaData[]>([])
  readonly analyzedVideos$ = this._analyzeVideos.asObservable();

  constructor(private mediaStorageService: MediaStorageService, private analysisService: AnalysisService) {
    this.fetchAll();
  }

  get analyzeVideos(): AnalyzedVideoMetaData[] {
    return this._analyzeVideos.getValue();
  }

  set analyzeVideos(val: AnalyzedVideoMetaData[]) {
    this._analyzeVideos.next(val);
  }

  async analyzeVideo(mediaId: string, pipelineId : string , mediaType: string = 'video'){
    await this.analysisService.analyzeMedia({mediaId: mediaId, pipelineId: pipelineId, mediaType: mediaType}).toPromise();
    this.fetchAll();
  }

  async fetchAll() {
    await this.mediaStorageService.getAnalyzedVideos().subscribe((res) => {
      this.analyzeVideos = res.videos;
    });
  }
}
