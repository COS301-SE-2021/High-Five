import {Injectable} from '@angular/core';
import {BehaviorSubject} from 'rxjs';
import {MediaStorageService} from '../../apis/mediaStorage.service';
import {VideoMetaData} from '../../models/videoMetaData';
import {HttpEventType} from '@angular/common/http';
import {SnotifyService} from 'ng-snotify';

@Injectable({
  providedIn: 'root'
})
export class VideosService {

  private readonly _videos = new BehaviorSubject<VideoMetaData[]>([]);
  // eslint-disable-next-line @typescript-eslint/member-ordering,no-underscore-dangle
  readonly videos$ = this._videos.asObservable();

  constructor(private mediaStorageService: MediaStorageService, private snotifyService: SnotifyService) {
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
      this.mediaStorageService.storeVideoForm(video, 'events', true).subscribe((ev) => {
        switch (ev.type) {
          case HttpEventType.UploadProgress:
            console.log(Math.round(ev.loaded / ev.total * 100));
            break;
          case HttpEventType.Response:
            this.snotifyService.success('Video upload', 'Successfully uploaded video');
            this.videos= this.videos.concat(ev.body);
            break;
        }
      });
    } catch (e) {
      this.snotifyService.error('Error occurred while uploading video, please contact an admin', 'Video upload');
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
        this.mediaStorageService.deleteVideo({id: videoId}, 'response').subscribe((res) => {
          if (res.ok) {
            this.snotifyService.success('Successfully removed video', 'Video Removal');
          } else {
            this.snotifyService.error('Error occurred while removing video, please contact an admin', 'Video Removal');

          }
        });
      } catch (e) {
        this.snotifyService.error('Error occurred while removing video, please contact an admin', 'Video Removal');
        console.error(e);
        this.videos = [...this.videos, video];
      }
    }
  }

  /**
   * Makes a request to retrieve all videos
   */
  public async fetchAll() {
    this.mediaStorageService.getAllVideos().subscribe((res) => {
      this.videos = res.videos;
    });
  }
}
