import {Injectable} from '@angular/core';
import {BehaviorSubject} from 'rxjs';
import {MediaStorageService} from '../../apis/mediaStorage.service';
import {ImageMetaData} from '../../models/imageMetaData';
import {SnotifyService} from 'ng-snotify';
import {HttpEvent, HttpEventType} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ImagesService {


  private readonly _images = new BehaviorSubject<ImageMetaData[]>([]);
  // eslint-disable-next-line @typescript-eslint/member-ordering,no-underscore-dangle
  readonly images$ = this._images.asObservable();

  constructor(private mediaStorageService: MediaStorageService, private snotifyService: SnotifyService) {
    this.fetchAll();
  }

  get images(): ImageMetaData[] {
    // eslint-disable-next-line no-underscore-dangle
    return this._images.getValue();
  }

  set images(val: ImageMetaData[]) {
    // eslint-disable-next-line no-underscore-dangle
    this._images.next(val);
  }

  /**
   * Uploads a video
   *
   * @param image the raw data of the video which must be uploaded
   */
  public async addImage(image: any) {
    try {
      this.mediaStorageService.storeImageForm(image, 'events', true).subscribe((ev: HttpEvent<any>) => {
        switch (ev.type) {
          case HttpEventType.UploadProgress:
            console.log(Math.round(ev.loaded / ev.total * 100));
            break;
          case HttpEventType.Response:
            this.snotifyService.success('Successfully uploaded image', 'Image upload');
            break;
        }

      });
    } catch (e) {
      this.snotifyService.error('Image upload failed, please contact an admin', 'Image Upload');
      console.log(e);
    }
  }


  /**
   * Removes an images either locally or locally and with a request to the backend
   *
   * @param imageId the id of the image which to remove (string)
   * @param serverRemove boolean, if true  will send a request to the backend to remove the image, otherwise
   * the image will only be removed locally, by default this parameter is set to true
   */
  public async removeImage(imageId: string, serverRemove: boolean = true) {
    const image = this.images.find(i => i.id === imageId);
    this.images = this.images.filter(i => i.id !== imageId);

    if (serverRemove) {
      try {
        await this.mediaStorageService.deleteImage({id: imageId}, 'response').subscribe((res) => {
          if (res.ok) {
            this.snotifyService.success('Successfully removed image', 'Image Removal');
          } else {
            this.snotifyService.error('Error occurred while removing image, please contact an admin', 'Image Removal');
            this.images = [...this.images, image];
          }
        });
      } catch (e) {
        this.snotifyService.error('Error occurred while removing image, please contact an admin', 'Image Removal');
        console.error(e);
        this.images = [...this.images, image];
      }
    }
  }

  /**
   * Makes a request to retrieve all images
   */
  public async fetchAll() {
    await this.mediaStorageService.getAllImages().subscribe((res) => {
      this.images = res.images;
    });
  }
}


