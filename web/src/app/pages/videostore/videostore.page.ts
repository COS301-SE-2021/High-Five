import {Component, OnInit, ViewChild} from '@angular/core';
import {IonInfiniteScroll, LoadingController, ModalController, ToastController} from '@ionic/angular';
import {VideoMetaData} from '../../models/videoMetaData';
import {VideoStoreConstants} from '../../../constants/pages/videostore-constants';
import {MediaService} from '../../services/videoupload/media.service';

@Component({
  selector: 'app-videostore',
  templateUrl: './videostore.page.html',
  styleUrls: ['./videostore.page.scss'],
})
export class VideostorePage implements OnInit {

  @ViewChild(IonInfiniteScroll) infiniteScroll: IonInfiniteScroll;

  public items: VideoMetaData[][] = [];
  public videosFetched = false;
  public segment;
  public images: string[]= new Array(20);

  constructor(private modal: ModalController, private videoService: MediaService,
              public toastController: ToastController,
              private loadingController: LoadingController,
              private constants: VideoStoreConstants) {
    this.loadInitData().then();
    this.segment='all';
  }

  ngOnInit() {
  }

  /**
   * Loads data from the backend once the 'video store' page has been loaded.
   */
  async loadInitData() {

    // show the spinner before fetching the data
    const loading = await this.loadingController.create({
      spinner: 'circles',
      animated:true,
    });
    await loading.present();

    // load the data, and pass in a callback function to close the spinner
    this.loadMoreData(async () => {
      await loading.dismiss();
    });
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
   *
   * @param func An optional callback function for when data is loaded.
   */
  loadMoreData(func: any = null) {
    this.videoService.getAllVideos(data => {
      // eslint-disable-next-line guard-for-in
      let row = true;
      let counter = 0; // keeps track of the current row
      for (const item of data) {

        // cards are presented as two-column rows. Therefore we swap between the first and second row
        // when populating the page.
        if (row) {
          this.items.push([Object.assign(new VideoMetaData(), item)]);
          row = false; //moves to the second column
        } else {
          this.items[counter].push(Object.assign(new VideoMetaData(), item));
          counter++; //moves to the next row
          row = true; //resets to the first column
        }
      }

      // If there is an odd number of items, then set the second column of the
      // last row to be undefined. Angular will not render it, as there is a check for an undefined column.
      if (!row) {
        this.items[counter].push(undefined);
      }

      this.videosFetched = true; // tell Angular to show the items.

      // calls the callback function if it has been set.
      if (func !== null) {
        func();
      }
    });
  }

  /**
   * Sends an uploaded video to the backend using the VideoUpload service.
   *
   * @param fileData
   */
  async uploadVideo(fileData: any) {

    // Load the spinner
    const loading = await this.loadingController.create({
      spinner: 'circles',
      animated:true,
    });
    await loading.present();

    // upload the video
    this.videoService.storeVideo(fileData.target.files[0].name, fileData.target.files[0], data => {
      console.log(data);
      this.presentAlert().then();
      this.items = [];
      this.loadMoreData();
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
   * Returns an anonymous functions for the child component to use when deleting itself.
   */
  getDeleteFunction() {
    return (vidId: string) => {

      // Updated list of videos
      const tmp: VideoMetaData[][] = [];
      let flip = false; // determines to which column the video goes to
      let counter = 0;
      for (const item of this.items) {
        for (const vid of item) {

          //Only add a video if it's id does not match the given ID.
          if (vid !== undefined && vid.id !== vidId) {
            if (!flip) {
              tmp.push([vid]);
              flip = true;
            } else {
              tmp[counter].push(vid);
              counter++;
              flip = false;
            }
          }
        }
      }

      // add an 'undefined' item so that Angular knows not to try add an empty column
      if (flip) {
        tmp[counter].push(undefined);
      }
      this.items = tmp;
      console.log(this.items);
    };
  }
}
