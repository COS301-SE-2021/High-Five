import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {MediaFilterComponent} from '../../components/media-filter/media-filter.component';
import {PopoverController} from '@ionic/angular';
import {UserPreferencesService} from '../../services/user-preferences/user-preferences.service';
import {VideosService} from '../../services/videos/videos.service';
import {ImagesService} from '../../services/images/images.service';

@Component({
  selector: 'app-media',
  templateUrl: './media.page.html',
  styleUrls: ['./media.page.scss'],
})
export class MediaPage implements OnInit {

  public mediaType: string;
  public filter: string;

  constructor(private router: Router, private popoverController: PopoverController,
              private userPreferencesService: UserPreferencesService, private videosService: VideosService,
              private imagesService: ImagesService) {
    this.mediaType = 'all';
    this.filter = 'all';
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
            this.filter= this.userPreferencesService.mediaFilter;
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
}
