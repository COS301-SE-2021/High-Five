import {Component, OnInit} from '@angular/core';
import {ImagesService} from '../../services/images/images.service';
import {VideosService} from '../../services/videos/videos.service';
import {AnalyzedVideosService} from '../../services/analyzed-videos/analyzed-videos.service';
import {UserPreferencesService} from '../../services/user-preferences/user-preferences.service';

@Component({
  selector: 'app-videostore',
  templateUrl: './videostore.page.html',
  styleUrls: ['./videostore.page.scss'],
})
export class VideostorePage implements OnInit {


  constructor(public imagesService: ImagesService, public videosService: VideosService,
              public analyzedVideosService: AnalyzedVideosService, public userPreferencesService: UserPreferencesService) {
  }

  public imagesTrackFn = (i, image) => image.id;
  public videoTrackFn = (v, video) => video.id;
  public analyzedVideoTrackFn = (av, analyzeVideo) => analyzeVideo.id;

  ngOnInit() {

  }


}
