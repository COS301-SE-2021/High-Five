import {Injectable} from '@angular/core';
import {BehaviorSubject} from 'rxjs';
import {MediaStorageService} from '../../apis/mediaStorage.service';
import {AnalyzedImageMetaData} from '../../models/analyzedImageMetaData';
import {AnalysisService} from '../../apis/analysis.service';
import {SnotifyService} from 'ng-snotify';

@Injectable({
  providedIn: 'root'
})
export class AnalyzedImagesService {

  private readonly _analyzedImages = new BehaviorSubject<AnalyzedImageMetaData[]>([]);
  // eslint-disable-next-line @typescript-eslint/member-ordering,no-underscore-dangle
  readonly analyzedImages$ = this._analyzedImages.asObservable();

  constructor(private mediaStorageService: MediaStorageService, private analysisService: AnalysisService,
              private snotifyService: SnotifyService) {
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
   * Makes a request to retrieve all analyzed images
   */
  public async fetchAll() {
    await this.mediaStorageService.getAnalyzedImages().subscribe((res) => {
      this.analyzedImages = res.images;
    });
  }

  public async deleteAnalyzedImage(imageId: string, serverRemove: boolean = true) {
    const analyzedImage = this.analyzedImages.find(i => i.id === imageId);
    this.analyzedImages = this.analyzedImages.filter(i => i.id !== imageId);

    if (serverRemove) {
      try {
        this.mediaStorageService.deleteAnalyzedImage({id: imageId}, 'response').subscribe((res) => {
          if (res.ok) {
            this.snotifyService.success('Successfully removed analyzed image', 'Analyzed Image Removal');
          } else {
            this.snotifyService.error('Error occurred while removing image, please contact an admin', 'Analyzed Image Removal');
            this.analyzedImages = [...this.analyzedImages, analyzedImage];
          }
        });
      } catch (e) {
        this.snotifyService.error('Error occurred while removing analyzed image, please contact an admin', 'Analyzed Image Removal');
        console.error(e);
        this.analyzedImages = [...this.analyzedImages, analyzedImage];
      }
    }
  }
}
