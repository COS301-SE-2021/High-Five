import { Injectable } from '@angular/core';
import {BehaviorSubject} from "rxjs";
import {MediaStorageService} from "../../apis/mediaStorage.service";
import {VideoMetaData} from "../../models/videoMetaData";

@Injectable({
  providedIn: 'root'
})
export class VideosService {

  private readonly _videos = new BehaviorSubject<VideoMetaData[]>([])
  readonly videos$ = this._videos.asObservable();

  constructor(private mediaStorageService: MediaStorageService) {
    this.fetchAll();
  }

  get videos(): VideoMetaData[] {
    return this._videos.getValue();
  }

  set videos(val: VideoMetaData[]) {
    this._videos.next(val);
  }

  async addVideo(video: any) {
    try {
      await this.mediaStorageService.storeVideoForm(video).toPromise();
      this.fetchAll();
    } catch (e) {
      console.log(e);
    }
  }

  async removeVideo(videoId: string, serverRemove: boolean = true) {
    const video = this.videos.find(v => v.id === videoId);
    this.videos = this.videos.filter(video => video.id !== videoId);

    if (serverRemove) {
      try {
        await this.mediaStorageService.deleteImage({id: videoId}).toPromise();
      } catch (e) {
        console.error(e);
        this.videos = [...this.videos, video];
      }
    }
  }

  async fetchAll() {
    await this.mediaStorageService.getAllVideos().subscribe((res) => {
      this.videos = res.videos;
    });
  }
}
