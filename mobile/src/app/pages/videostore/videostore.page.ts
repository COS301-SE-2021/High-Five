import {Component, OnInit, ViewChild} from '@angular/core';
import {IonInfiniteScroll, ModalController} from '@ionic/angular';
import {VideouploadService} from '../../services/videoupload/videoupload.service';
import {GetAllVideosResponse} from '../../models/getAllVideosResponse';
import {VideoMetaData} from '../../models/videoMetaData';

@Component({
  selector: 'app-videostore',
  templateUrl: './videostore.page.html',
  styleUrls: ['./videostore.page.scss'],
})
export class VideostorePage implements OnInit {

  @ViewChild(IonInfiniteScroll) infiniteScroll: IonInfiniteScroll;

  public items: VideoMetaData[] = [];

  constructor(private modal: ModalController, private videoService: VideouploadService) {
    this.loadMoreData();
  }

  ngOnInit() {
  }

  loadData(event) {
    setTimeout(async () => {
      this.loadMoreData();
      event.target.complete();
    }, 500);
  }

  loadMoreData() {
    this.videoService.getAllVideos(data => {
      // eslint-disable-next-line guard-for-in
      for (const item of data) {
        this.items.push(Object.assign(new VideoMetaData(), item));
      }
    });
  }

  uploadVideo(fileData: any) {
    console.log(fileData.target.files[0]);
  }

}
