import { Injectable } from '@angular/core';
import {BehaviorSubject} from "rxjs";
import {MediaStorageService} from "../../apis/mediaStorage.service";
import {AnalyzedImageMetaData} from "../../models/analyzedImageMetaData";
import {AnalysisService} from "../../apis/analysis.service";

@Injectable({
  providedIn: 'root'
})
export class AnalyzedImagesService {

  private readonly _analyzedImages = new BehaviorSubject<AnalyzedImageMetaData[]>([])
  readonly analyzedImages$ = this._analyzedImages.asObservable();

  constructor(private mediaStorageService: MediaStorageService, private analysisService : AnalysisService) {
    this.fetchAll();
  }

  get analyzedImages(): AnalyzedImageMetaData[] {
    return this._analyzedImages.getValue();
  }

  set analyzedImages(val: AnalyzedImageMetaData[]) {
    this._analyzedImages.next(val);
  }

  async analyzeImage(mediaId: string, pipelineId : string , mediaType: string = 'image'){
    await this.analysisService.analyzeMedia({mediaId: mediaId, pipelineId: pipelineId, mediaType: mediaType}).toPromise();
    this.fetchAll();
  }
  async fetchAll() {
    await this.mediaStorageService.getAnalyzedImages().subscribe((res) => {
      this.analyzedImages = res.images;
    });
  }
}
