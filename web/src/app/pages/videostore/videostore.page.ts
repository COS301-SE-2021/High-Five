import {Component, OnInit, ViewChild} from '@angular/core';
import {IonInfiniteScroll, LoadingController, ModalController, ToastController} from '@ionic/angular';
import {VideoMetaData} from '../../models/videoMetaData';
import {VideoStoreConstants} from '../../../constants/pages/videostore-constants';
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

  constructor(private modal: ModalController,
              public toastController: ToastController,
              private loadingController: LoadingController,
              private constants: VideoStoreConstants, private mediaStorageService: MediaStorageService) {
    this.segment = 'images';
  }

  ngOnInit() {
    this.updateImages().then(() => {
    });
    this.updateVideos().then(() => {
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

  /**
   * This function will delete an image from the user's account, optimistic loading updates are used and in the event
   * and error is thrown, the image is added back and an appropriate toast is shown
   *
   * @param imageId the ID of the image we wish to delete
   */
  deleteImage(imageId: string) {
    if(this.images.length==0){
      return;
    }
    this.images = this.images.filter(img => img.id !== imageId);
    const image: ImageMetaData = this.images.filter(img => img.id === imageId)[0];
    try {
      this.mediaStorageService.deleteImage({id: imageId}).subscribe(() => {
        this.toastController.create({
          message: 'Successfully deleted image',
          duration: 2000,
          translucent: true
        }).then(m => m.present());
      });
    } catch (e) {
      this.toastController.create({
        message: 'Error occurred while deleting image',
        duration: 2000,
        translucent: true
      }).then(m => m.present());
      this.images = this.images.concat([image]);
      if(this.images != undefined && this.images.length>1){
        this.images.sort((a, b) => a.name.localeCompare(b.name));
      }
    }

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
      this.images = response.images;
      if(this.images != undefined  && this.images.length>1){
        this.images.sort((a, b) => a.name.localeCompare(b.name));
      }
      return true;
    });
    return false;
  }

  private async updateVideos(): Promise<boolean> {
    this.mediaStorageService.getAllVideos().subscribe(response => {
      this.videos = response.videos;
      if(this.videos != undefined  && this.videos.length>1){
        this.videos.sort((a, b) => a.name.localeCompare(b.name));
      }
      return true;
    });
    return false;
  }

}
