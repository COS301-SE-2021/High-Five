import {Component, OnInit, ViewChild} from '@angular/core';
import {IonInfiniteScroll, ModalController, ToastController} from '@ionic/angular';
import {VideouploadService} from '../../services/videoupload/videoupload.service';
import {VideoMetaData} from '../../models/videoMetaData';

@Component({
  selector: 'app-videostore',
  templateUrl: './videostore.page.html',
  styleUrls: ['./videostore.page.scss'],
})
export class VideostorePage implements OnInit {

  @ViewChild(IonInfiniteScroll) infiniteScroll: IonInfiniteScroll;

  public items: VideoMetaData[][] = [];
  public videosFetched = false;

  constructor(private modal: ModalController, private videoService: VideouploadService, public alertController: ToastController) {
    this.loadMoreData();
  }

  ngOnInit() {
  }

  /**
   * Called when Ionic's infinite scroll wants to load more data.
   *
   * @param event
   */
  loadData(event) {
    setTimeout(async () => {
      this.loadMoreData();
      event.target.complete();
    }, 500);
  }

  /**
   * Fetches video metadata from the backend and adds the data to the 'item' list.
   */
  loadMoreData() {
    this.videoService.getAllVideos(data => {
      // eslint-disable-next-line guard-for-in
      let row = true;
      let counter = 0;
      for (const item of data) {
        if (row) {
          this.items.push([Object.assign(new VideoMetaData(), item)]);
          row = false;
        } else {
          this.items[counter].push(Object.assign(new VideoMetaData(), item));
          counter++;
          row = true;
        }
      }
      if (!row) {
        this.items[counter].push(undefined);
      }
      this.videosFetched = true;
    });
  }

  /**
   * Sends an uploaded video to the backend.
   *
   * @param fileData
   */
  uploadVideo(fileData: any) {
    this.videosFetched = false;
    this.videoService.storeVideo(fileData.target.files[0].name, fileData.target.files[0], data => {
      console.log(data);
      this.presentAlert().then();
      this.items = [];
      this.loadMoreData();
    });
  }

  async presentAlert() {
    const alert = await this.alertController.create({
      cssClass: 'alert-style',
      header: 'Video Uploaded',
      message: 'Video successfully uploaded.',
      buttons: ['OK']
    });

    await alert.present();
  }
}
