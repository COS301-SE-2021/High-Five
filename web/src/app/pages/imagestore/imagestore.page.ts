import { Component, OnInit } from '@angular/core';
import {ImagesService} from "../../services/images/images.service";
import {AnalyzedImagesService} from "../../services/analyzed-images/analyzed-images.service";

@Component({
  selector: 'app-imagestore',
  templateUrl: './imagestore.page.html',
  styleUrls: ['./imagestore.page.scss'],
})
export class ImagestorePage implements OnInit {
  analyzedImageTrackFn = (ai, analyzed_image) => analyzed_image.id;
  imageTrackFn= (i, image) => image.id;
  segment : string;
  constructor(public imagesService : ImagesService, public  analyzedImagesService : AnalyzedImagesService) {
    this.segment='all'
  }
  ngOnInit() {
  }
  /**
   * This function will delete an image from the user's account, optimistic loading updates are used and in the event
   * and error is thrown, the image is added back and an appropriate toast is shown
   *
   * @param image the data of the image we wish to upload
   */

  async uploadImage(image: any) {

    await this.imagesService.addImage(image.target.files[0]);
    //Nothing added here yet
  }
}
