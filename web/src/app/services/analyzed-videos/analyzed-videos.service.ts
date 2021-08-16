import { Injectable } from '@angular/core';
import {BehaviorSubject} from "rxjs";
import {MediaStorageService} from "../../apis/mediaStorage.service";
import {AnalyzedVideoMetaData} from "../../models/analyzedVideoMetaData";

@Injectable({
  providedIn: 'root'
})
export class AnalyzedVideosService {


  private readonly _analyzeVideos = new BehaviorSubject<AnalyzedVideoMetaData[]>([])
  readonly analyzedVideos$ = this._analyzeVideos.asObservable();

  constructor(private mediaStorageService: MediaStorageService) {
    this.fetchAll();
  }

  get analyzeVideos(): AnalyzedVideoMetaData[] {
    return this._analyzeVideos.getValue();
  }

  set analyzeVideos(val: AnalyzedVideoMetaData[]) {
    this._analyzeVideos.next(val);
  }

  async fetchAll() {
    await this.mediaStorageService.getAnalyzedVideos().subscribe((res) => {
      this.analyzeVideos = res.videos;
    });
  }
}
