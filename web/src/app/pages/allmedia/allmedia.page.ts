import {Component, OnInit} from '@angular/core';
import {VideosService} from '../../services/videos/videos.service';
import {ImagesService} from '../../services/images/images.service';
import {AnalyzedVideosService} from '../../services/analyzed-videos/analyzed-videos.service';
import {AnalyzedImagesService} from '../../services/analyzed-images/analyzed-images.service';
import {PopoverController} from '@ionic/angular';
import {MediaFilterComponent} from '../../components/media-filter/media-filter.component';

@Component({
  selector: 'app-allmedia',
  templateUrl: './allmedia.page.html',
  styleUrls: ['./allmedia.page.scss'],
})
export class AllmediaPage implements OnInit {
  public segment: string;


  constructor(public videosService: VideosService, public imagesService: ImagesService,
              public analyzedVideosService: AnalyzedVideosService, public analyzedImagesService: AnalyzedImagesService,
              private popoverController: PopoverController) {
    this.segment = 'all';
  }

  public analyzedImageTrackFn = (ai, analyzedImage) => analyzedImage.id;
  public imageTrackFn = (i, image) => image.id;
  public videoTrackFn = (v, video) => video.id;
  public analyzedVideoTrackFn = (av, analyzedVideo) => analyzedVideo.id;

  ngOnInit() {
  }


  /**
   * Sends an uploaded video to the backend using the videosService.
   *
   * @param video, the data of the video that is going to be uploaded
   */
  public async uploadVideo(video: any) {
    await this.videosService.addVideo(video.target.files[0]);
  }


  /**
   * Sends an uploaded image to the backend to be saved using the imagesService
   *
   * @param image, the data of the image that is going to be uploaded
   */
  public async uploadImage(image: any) {
    await this.imagesService.addImage(image.target.files[0]);
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
            this.segment = data.data.segment;
          }
        }
      }
    );
  }
}
