import {Component, OnInit, ViewChild} from '@angular/core';
import {
  IonInfiniteScroll,
  LoadingController,
  ModalController,
  PopoverController,
  ToastController
} from '@ionic/angular';
import {VideoStoreConstants} from '../../../constants/pages/videostore-constants';
import {ImagesService} from '../../services/images/images.service';
import {VideosService} from '../../services/videos/videos.service';
import {AnalyzedVideosService} from '../../services/analyzed-videos/analyzed-videos.service';
import {MediaFilterComponent} from '../../components/media-filter/media-filter.component';

@Component({
  selector: 'app-videostore',
  templateUrl: './videostore.page.html',
  styleUrls: ['./videostore.page.scss'],
})
export class VideostorePage implements OnInit {

  @ViewChild(IonInfiniteScroll) infiniteScroll: IonInfiniteScroll;

  public segment: string;
  public analyzed: boolean;

  constructor(private modal: ModalController,
              public toastController: ToastController,
              private loadingController: LoadingController,
              private constants: VideoStoreConstants, public imagesService: ImagesService,
              public videosService: VideosService, private popoverController: PopoverController,
              public analyzedVideosService: AnalyzedVideosService,) {
    this.segment = 'all';
  }

  public imagesTrackFn = (i, image) => image.id;
  public videoTrackFn = (v, video) => video.id;
  public analyzedVideoTrackFn = (av, analyzeVideo) => analyzeVideo.id;

  ngOnInit() {

  }


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
   * Sends an uploaded video to the backend using the videosService service.
   *
   * @param video
   */
  public async uploadVideo(video: any) {
    await this.videosService.addVideo(video.target.files[0]);
  }

}
