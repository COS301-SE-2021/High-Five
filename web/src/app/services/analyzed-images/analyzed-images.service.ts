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

  /**
   * Getter for analyzed images
   */
  get analyzedImages(): AnalyzedImageMetaData[] {
    // eslint-disable-next-line no-underscore-dangle
    return this._analyzedImages.getValue();
  }

  /**
   * Setter for analyzedImages
   *
   * @param val the value to which the _analysedImages property will be set
   */
  set analyzedImages(val: AnalyzedImageMetaData[]) {
    // eslint-disable-next-line no-underscore-dangle
    this._analyzedImages.next(val);
  }

  /**
   * Function will send a request to analyze a media with the specified mediaId, pipelineId and media type
   *
   * @param mediaId the id of the media which to analyze (video or image)
   * @param pipelineId the id of the pipeline with which to analyze the media
   * @param mediaType the media type, video or image
   */
  public async analyzeImage(mediaId: string, pipelineId: string, mediaType: string = 'image') {
    await this.analysisService.analyzeMedia({mediaId, pipelineId, mediaType}).toPromise();
    await this.fetchAll();
  }

  /**
   * Makes a request to retrieve all analyzed images
   */
  public async fetchAll() {
    await this.mediaStorageService.getAnalyzedImages().subscribe((res) => {
      this.analyzedImages = res.images;
    });
  }
}
