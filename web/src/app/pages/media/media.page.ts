import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {MediaFilterComponent} from '../../components/media-filter/media-filter.component';
import {PopoverController} from '@ionic/angular';
import {UserPreferencesService} from '../../services/user-preferences/user-preferences.service';
import {VideosService} from '../../services/videos/videos.service';
import {ImagesService} from '../../services/images/images.service';
import {MediaStorageService} from '../../apis/mediaStorage.service';
import {HttpEventType} from '@angular/common/http';

@Component({
  selector: 'app-media',
  templateUrl: './media.page.html',
  styleUrls: ['./media.page.scss'],
})
export class MediaPage implements OnInit {

  public mediaType: string;
  public filter: string;
  public uploading: boolean;
  // public uploadProgress: BehaviorSubject<number> = new BehaviorSubject<number>(0);
  public uploadProgress: number;

  constructor(private router: Router, private popoverController: PopoverController,
              private userPreferencesService: UserPreferencesService, private videosService: VideosService,
              private imagesService: ImagesService, private mediaStorageService: MediaStorageService) {
    this.mediaType = 'all';
    this.filter = 'all';
    this.uploading = false;
    this.uploadProgress = 0;
  }

  segmentChange(ev: any) {
    this.router.navigate(['/navbar/media/' + this.mediaType]);
  }

  ngOnInit() {
  }

  /**
   * Displays a popover that contains the filter options
   *
   * @param ev the event which is required by the popover
   */
  public async displayFilterPopover(ev: any) {
    const filterPopover = await this.popoverController.create({
      component: MediaFilterComponent,
      cssClass: 'media-filter',
      animated: true,
      translucent: true,
      backdropDismiss: true,
      event: ev,
    });
    await filterPopover.present();
    await filterPopover.onDidDismiss().then(
      data => {
        /**
         * The below is to ensure the popover isn't dismissed from the backdrop, in which case the data.data and
         * data.data.segment will be undefined.
         */
        if (data.data !== undefined) {
          if (data.data.segment !== undefined) {
            this.userPreferencesService.mediaFilter = data.data.segment;
            this.filter = this.userPreferencesService.mediaFilter;
          }
        }
      }
    );
  }

  /**
   * Sends an uploaded video to the backend using the videosService.
   *
   * @param video, the data of the video that is going to be uploaded
   */
  public async uploadVideo(video: any) {
    try {
      this.mediaStorageService.storeVideoForm(video.target.files[0], 'events', true).subscribe((ev) => {
        switch (ev.type) {
          case HttpEventType.UploadProgress:
            this.uploading = true;
            this.uploadProgress = ev.loaded / ev.total;
            break;
          case HttpEventType.Response:
            this.videosService.addVideoModel(ev.body);
            this.uploading = false;
            this.uploadProgress = 0;
            break;
          case HttpEventType.Sent:
            this.uploading = false;
            this.uploadProgress = 0;
            break;
          case HttpEventType.ResponseHeader:
            this.uploading = false;
            this.uploadProgress= 0;
            break;
        }
      });
    } catch (e) {
      this.uploading = false;
      this.uploadProgress = 0;
    }


    // await this.videosService.addVideo(video.target.files[0]);
  }


  /**
   * Sends an uploaded image to the backend to be saved using the imagesService
   *
   * @param image, the data of the image that is going to be uploaded
   */
  public async uploadImage(image: any) {
    await this.imagesService.addImage(image.target.files[0]);
  }
}
