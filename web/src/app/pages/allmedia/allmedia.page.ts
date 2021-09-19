import {Component, OnInit} from '@angular/core';
import {VideosService} from '../../services/videos/videos.service';
import {ImagesService} from '../../services/images/images.service';
import {AnalyzedVideosService} from '../../services/analyzed-videos/analyzed-videos.service';
import {AnalyzedImagesService} from '../../services/analyzed-images/analyzed-images.service';
import {UserPreferencesService} from '../../services/user-preferences/user-preferences.service';

@Component({
  selector: 'app-allmedia',
  templateUrl: './allmedia.page.html',
  styleUrls: ['./allmedia.page.scss'],
})
export class AllmediaPage implements OnInit {
  public segment: string;


  constructor(public videosService: VideosService, public imagesService: ImagesService,
              public analyzedVideosService: AnalyzedVideosService, public analyzedImagesService: AnalyzedImagesService,
              public userPreferencesService: UserPreferencesService) {
  }


  public analyzedImageTrackFn = (ai, analyzedImage) => analyzedImage.id;
  public imageTrackFn = (i, image) => image.id;
  public videoTrackFn = (v, video) => video.id;
  public analyzedVideoTrackFn = (av, analyzedVideo) => analyzedVideo.id;

  ngOnInit() {
  }


}
