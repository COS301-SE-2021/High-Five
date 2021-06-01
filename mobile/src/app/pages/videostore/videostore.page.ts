import {Component, OnInit, ViewChild} from '@angular/core';
import { IonInfiniteScroll } from '@ionic/angular';

@Component({
  selector: 'app-videostore',
  templateUrl: './videostore.page.html',
  styleUrls: ['./videostore.page.scss'],
})
export class VideostorePage implements OnInit {

  @ViewChild(IonInfiniteScroll) infiniteScroll: IonInfiniteScroll;

  constructor() {
    this.loadMoreData();
  }

  items : VideoPreviewData[][] = []

  ngOnInit() {
  }

  loadData(event) {
    setTimeout(async () => {
      this.loadMoreData();
      event.target.complete();
    }, 500);
  }

  loadMoreData() {
    for (let i = 0; i < 10; i++) {
      this.items.push([
        new VideoPreviewData('Test Title', new Date('2021-01-01'), 'https://source.unsplash.com/random/200x200?sig=' + i),
        new VideoPreviewData('Test Title', new Date('2021-01-01'), 'https://source.unsplash.com/random/200x200?sig=' + (i+1))
      ])
    }
  }

}

export class VideoPreviewData {
  private title : string
  private recordedDate : Date
  private imageUrl : string

  constructor(title : string, date: Date, imageUrl : string) {
    this.title = title;
    this.recordedDate = date;
    this.imageUrl = imageUrl;
  }

  getTitle() : string {
    return this.title;
  }

  getRecordedDate() : Date {
    return this.recordedDate;
  }

  getImageUrl() : string {
    return this.imageUrl;
  }
}