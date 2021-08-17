import { Component, OnInit } from '@angular/core';
import {ImagesService} from "../../services/images/images.service";
import {AnalyzedImagesService} from "../../services/analyzed-images/analyzed-images.service";
import {MediaFilterComponent} from "../../components/media-filter/media-filter.component";
import {PopoverController} from "@ionic/angular";

@Component({
  selector: 'app-imagestore',
  templateUrl: './imagestore.page.html',
  styleUrls: ['./imagestore.page.scss'],
})
export class ImagestorePage implements OnInit {
  analyzedImageTrackFn = (ai, analyzed_image) => analyzed_image.id;
  imageTrackFn= (i, image) => image.id;
  segment : string;
  constructor(public imagesService : ImagesService, public  analyzedImagesService : AnalyzedImagesService,
              private popoverController : PopoverController) {
    this.segment='all'
  }
  ngOnInit() {
  }

  async displayFilterPopover(ev: any) {
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
        if (data.data != undefined) {
          if (data.data.segment != undefined) {
            this.segment = data.data.segment;
          }
        }
      }
    );
  }

  async uploadImage(image: any) {

    await this.imagesService.addImage(image.target.files[0]);
    //Nothing added here yet
  }


}
