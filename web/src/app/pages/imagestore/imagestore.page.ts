import {Component, OnInit} from '@angular/core';
import {ImagesService} from '../../services/images/images.service';
import {AnalyzedImagesService} from '../../services/analyzed-images/analyzed-images.service';
import {MediaFilterComponent} from '../../components/media-filter/media-filter.component';
import {PopoverController} from '@ionic/angular';

@Component({
  selector: 'app-imagestore',
  templateUrl: './imagestore.page.html',
  styleUrls: ['./imagestore.page.scss'],
})
export class ImagestorePage implements OnInit {

  public segment: string;

  constructor(public imagesService: ImagesService, public analyzedImagesService: AnalyzedImagesService,
              private popoverController: PopoverController) {
    //Setting the segment's property to all, ensures that on page load, all media is shown
    this.segment = 'all';
  }

  public analyzedImageTrackFn = (ai, analyzedImage) => analyzedImage.id;
  public imageTrackFn = (i, image) => image.id;

  ngOnInit() {
  }

  /**
   * Displays a popover that contains the filter options, present in the MediaFilterComponent
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
        if (data.data !== undefined) {
          if (data.data.segment !== undefined) {
            this.segment = data.data.segment;
          }
        }
      }
    );
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
