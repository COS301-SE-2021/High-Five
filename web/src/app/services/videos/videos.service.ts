import {Injectable} from '@angular/core';
import {BehaviorSubject} from 'rxjs';
import {MediaStorageService} from '../../apis/mediaStorage.service';
import {VideoMetaData} from '../../models/videoMetaData';

@Injectable({
  providedIn: 'root'
})
export class VideosService {

  private readonly _videos = new BehaviorSubject<VideoMetaData[]>([]);
  // eslint-disable-next-line @typescript-eslint/member-ordering,no-underscore-dangle
  readonly videos$ = this._videos.asObservable();

  constructor(private mediaStorageService: MediaStorageService) {
    this.fetchAll();
  }

  get videos(): VideoMetaData[] {
    // eslint-disable-next-line no-underscore-dangle
    return this._videos.getValue();
  }

  set videos(val: VideoMetaData[]) {
    // eslint-disable-next-line no-underscore-dangle
    this._videos.next(val);
  }

  /**
   * Uploads a video
   *
   * @param video the raw data of the video which must be uploaded
   */
  public async addVideo(video: any) {
    try {
      await this.mediaStorageService.storeVideoForm(video).toPromise();
      this.fetchAll();
    } catch (e) {
      console.log(e);
    }
  }

  /**
   *
   * @param videoId the id of the video which to remove (string)
   * @param serverRemove boolean, if true  will send a request to the backend to remove the video, otherwise
   * the video will only be removed locally, by default this parameter is set to true
   */
  public async removeVideo(videoId: string, serverRemove: boolean = true) {
    const video = this.videos.find(v => v.id === videoId);
    this.videos = this.videos.filter(v => v.id !== videoId);

    if (serverRemove) {
      try {
        await this.mediaStorageService.deleteVideo({id: videoId}).toPromise();
      } catch (e) {
        console.error(e);
        this.videos = [...this.videos, video];
      }
    }
  }

  /**
   * Makes a request to retrieve all videos
   */
  public async fetchAll() {
    await this.mediaStorageService.getAllVideos().subscribe((res) => {
      this.videos = res.videos;
    });
  }
}
