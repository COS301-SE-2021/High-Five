import {Injectable} from '@angular/core';
import {BehaviorSubject} from 'rxjs';
import {MediaStorageService} from '../../apis/mediaStorage.service';
import {AnalyzedImageMetaData} from '../../models/analyzedImageMetaData';
import {AnalysisService} from '../../apis/analysis.service';

@Injectable({
  providedIn: 'root'
})
export class AnalyzedImagesService {

  private readonly _analyzedImages = new BehaviorSubject<AnalyzedImageMetaData[]>([]);
  // eslint-disable-next-line @typescript-eslint/member-ordering,no-underscore-dangle
  readonly analyzedImages$ = this._analyzedImages.asObservable();

  constructor(private mediaStorageService: MediaStorageService, private analysisService: AnalysisService) {
    this.fetchAll();
  }

  get analyzedImages(): AnalyzedImageMetaData[] {
    // eslint-disable-next-line no-underscore-dangle
    return this._analyzedImages.getValue();
  }

  set analyzedImages(val: AnalyzedImageMetaData[]) {
    // eslint-disable-next-line no-underscore-dangle
    this._analyzedImages.next(val);
  }

  public async analyzeImage(mediaId: string, pipelineId: string, mediaType: string = 'image') {
    await this.analysisService.analyzeMedia({mediaId, pipelineId, mediaType}).toPromise();
    await this.fetchAll();
  }

  public async fetchAll() {
    await this.mediaStorageService.getAnalyzedImages().subscribe((res) => {
      this.analyzedImages = res.images;
    });
  }
}
