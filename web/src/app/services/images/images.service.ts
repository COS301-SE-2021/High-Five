import {Injectable} from '@angular/core';
import {BehaviorSubject} from "rxjs";
import {MediaStorageService} from "../../apis/mediaStorage.service";
import {ImageMetaData} from "../../models/imageMetaData";

@Injectable({
  providedIn: 'root'
})
export class ImagesService {

  private readonly _images = new BehaviorSubject<ImageMetaData[]>([])
  readonly images$ = this._images.asObservable();

  constructor(private mediaStorageService: MediaStorageService) {
    this.fetchAll();
  }

  get images(): ImageMetaData[] {
    return this._images.getValue();
  }

  set images(val: ImageMetaData[]) {
    this._images.next(val);
  }

  async addImage(image: any) {
    try {
      await this.mediaStorageService.storeImageForm(image).toPromise();
      this.fetchAll();
    } catch (e) {
      console.log(e);
    }
  }

  async removeImage(imageId: string, serverRemove: boolean = true) {
    const image = this.images.find(i => i.id === imageId);
    this.images = this.images.filter(image => image.id !== imageId);

    if (serverRemove) {
      try {
        await this.mediaStorageService.deleteImage({id: imageId}).toPromise();
      } catch (e) {
        console.error(e);
        this.images = [...this.images, image];
      }
    }
  }

  async fetchAll() {
    await this.mediaStorageService.getAllImages().subscribe((res) => {
      this.images = res.images;
    });
  }
}


