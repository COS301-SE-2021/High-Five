import { Injectable } from '@angular/core';
import {BehaviorSubject} from "rxjs";
import {MediaStorageService} from "../../apis/mediaStorage.service";
import {AnalyzedImageMetaData} from "../../models/analyzedImageMetaData";

@Injectable({
  providedIn: 'root'
})
export class AnalyzedImagesService {

  private readonly _analyzedImages = new BehaviorSubject<AnalyzedImageMetaData[]>([])
  readonly analyzedImages$ = this._analyzedImages.asObservable();

  constructor(private mediaStorageService: MediaStorageService) {
    this.fetchAll();
  }

  get analyzedImages(): AnalyzedImageMetaData[] {
    return this._analyzedImages.getValue();
  }

  set analyzedImages(val: AnalyzedImageMetaData[]) {
    this._analyzedImages.next(val);
  }


  async fetchAll() {
    await this.mediaStorageService.getAnalyzedImages().subscribe((res) => {
      this.analyzedImages = res.images;
    });
  }
}
