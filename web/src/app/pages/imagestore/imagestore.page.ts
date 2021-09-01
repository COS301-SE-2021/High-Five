import {Component, OnInit} from '@angular/core';
import {ImagesService} from '../../services/images/images.service';
import {AnalyzedImagesService} from '../../services/analyzed-images/analyzed-images.service';
import {UserPreferencesService} from '../../services/user-preferences/user-preferences.service';

@Component({
  selector: 'app-imagestore',
  templateUrl: './imagestore.page.html',
  styleUrls: ['./imagestore.page.scss'],
})
export class ImagestorePage implements OnInit {


  constructor(public imagesService: ImagesService, public analyzedImagesService: AnalyzedImagesService,
              public userPreferencesService: UserPreferencesService) {

  }

  public analyzedImageTrackFn = (ai, analyzedImage) => analyzedImage.id;
  public imageTrackFn = (i, image) => image.id;

  ngOnInit() {
  }

}
