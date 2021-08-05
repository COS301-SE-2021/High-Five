import {Component, OnInit, ViewChild} from '@angular/core';
import {IonInfiniteScroll, LoadingController, ModalController, ToastController} from '@ionic/angular';
import {VideoMetaData} from '../../models/videoMetaData';
import {VideoStoreConstants} from '../../../constants/pages/videostore-constants';
import {MediaService} from '../../services/media/media.service';
import {ImageMetaData} from '../../models/imageMetaData';
import {MediaStorageService} from '../../apis/mediaStorage.service';

@Component({
  selector: 'app-videostore',
  templateUrl: './videostore.page.html',
  styleUrls: ['./videostore.page.scss'],
})
export class VideostorePage implements OnInit {

  @ViewChild(IonInfiniteScroll) infiniteScroll: IonInfiniteScroll;

  public videos: VideoMetaData[] = [];
  public videosFetched = false;
  public segment: string;
  public images: ImageMetaData[] = [];

  constructor(private modal: ModalController, private videoService: MediaService,
              public toastController: ToastController,
              private loadingController: LoadingController,
              private constants: VideoStoreConstants, private mediaStorageService: MediaStorageService) {
    this.segment = 'images';
  }

  ngOnInit() {
    //Nothing added here yet
    this.updateImages().then(() => {
      console.log(this.videos);
    });
    this.updateVideos().then(() => {
      console.log(this.videos);
    });
  }


  deleteVideo(videoId: string) {
    this.videos = this.videos.filter(video => video.id !== videoId);
    this.mediaStorageService.deleteVideo({id: videoId}).subscribe(() => {
      this.toastController.create({
        message: 'Successfully deleted video',
        duration: 2000,
        translucent: true
      }).then(m => m.present());
    });

  }

  /**
   * Sends an uploaded video to the backend using the VideoUpload service.
   *
   * @param video
   */
  async uploadVideo(video: any) {

    const loading = await this.loadingController.create({
      spinner: 'circles',
      animated: true,
    });
    await loading.present();
    this.mediaStorageService.storeVideoForm(video.target.files[0]).subscribe(() => {
      this.updateVideos();
      loading.dismiss();
    });
  }

  /**
   * Shows a toast once a video is successfully uploaded.
   */
  async presentAlert() {
    const alert = await this.toastController.create({
      cssClass: 'alert-style',
      header: this.constants.toastLabels.header,
      message: this.constants.toastLabels.message,
      buttons: this.constants.toastLabels.buttons
    });
    await alert.present();
  }

  onDeleteImage(imageId: string) {
    this.images = this.images.filter(img => img.id !== imageId);
    this.mediaStorageService.deleteImage({id: imageId}).subscribe(() => {
      this.toastController.create({
        message: 'Successfully deleted video',
        duration: 2000,
        translucent: true
      }).then(m => m.present());
    });
  }

  async uploadImage(image: any) {
    const loading = await this.loadingController.create({
      spinner: 'circles',
      animated: true,
    });
    await loading.present();
    this.mediaStorageService.storeImageForm(image.target.files[0]).subscribe(() => {
      this.updateImages();
      loading.dismiss();
    });
    //Nothing added here yet

  }

  private async updateImages(): Promise<boolean> {
    this.mediaStorageService.getAllImages().subscribe(response => {
      console.log(response);
      this.images = response.images;
      this.images.sort((a, b) => a.name.localeCompare(b.name));
      return true;
    });
    return false;
  }

  private async updateVideos(): Promise<boolean> {
    this.mediaStorageService.getAllVideos().subscribe(response => {
      console.log(response);
      this.videos = response.videos;
      this.videos.sort((a, b) => a.name.localeCompare(b.name));
      return true;
    });
    return false;
  }

}
