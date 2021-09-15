import {Injectable} from '@angular/core';
import {WebSocketSubject} from 'rxjs/internal-compatibility';
import {environment} from '../../../environments/environment';
import {webSocket} from 'rxjs/webSocket';
import {JsonObject} from '@angular/compiler-cli/ngcc/src/packages/entry_point';
import {SnotifyService} from 'ng-snotify';
import {AnalyzedVideosService} from '../analyzed-videos/analyzed-videos.service';
import {AnalyzedImagesService} from '../analyzed-images/analyzed-images.service';
import {LiveStreamingService} from '../live-streaming/live-streaming.service';


@Injectable({
  providedIn: 'root'
})
export class WebsocketService {
  private socket: WebSocketSubject<any>;


  constructor(private ngSnotify: SnotifyService, private analyzedVideosService: AnalyzedVideosService,
              private analyzedImagesService: AnalyzedImagesService, private liveStreamingService: LiveStreamingService) {
    this.socket = webSocket({
      url: environment.websocketEndpoint, closeObserver: {
        // eslint-disable-next-line prefer-arrow/prefer-arrow-functions
        next(closeEvent) {
          const customError = {code: 6666, reason: 'Custom reason message'};
          console.log(`code: ${customError.code}, reason: ${customError.reason}`);
        }
      }
    });
    this.socket.subscribe((msg) => {
      console.log(msg);
      this.handleMessage(msg);
    });
    this.sendMessage({
      // eslint-disable-next-line @typescript-eslint/naming-convention
      Request: 'Synchronize'
    });
  }

  public sendMessage(message: JsonObject) {
    message.Authorization = JSON.parse(sessionStorage.getItem(sessionStorage.key(0))).secret;
    this.socket.next(message);
  }

  /**
   * Function will send a request to analyze a media with the specified mediaId, pipelineId and media type
   *
   * @param mediaId the id of the media which to analyze (video or image)
   * @param pipelineId the id of the pipeline with which to analyze the media
   * @param mediaType the media type, video or image
   */
  public async analyzeImage(mediaId: string, pipelineId: string, mediaType: string = 'image') {
    this.sendMessage({
      // eslint-disable-next-line @typescript-eslint/naming-convention
      Request: 'AnalyzeImage',
      // eslint-disable-next-line @typescript-eslint/naming-convention
      Body: {
        imageId: mediaId,
        pipelineId,
      },
    });
  }

  /**
   * Function will send a request to analyze a media with the specified mediaId, pipelineId and media type
   *
   * @param mediaId the id of the media which to analyze (video or image)
   * @param pipelineId the id of the pipeline with which to analyze the media
   * @param mediaType the media type, video or image
   */
  public async analyzeVideo(mediaId: string, pipelineId: string, mediaType: string = 'video') {
    this.sendMessage({
      // eslint-disable-next-line @typescript-eslint/naming-convention
      Request: 'AnalyzeVideo',
      // eslint-disable-next-line @typescript-eslint/naming-convention
      Body: {
        imageId: mediaId,
        pipelineId,
      },
    });
  }

  private handleMessage(msg: JsonObject) {
    console.log(msg);
    if (msg.type === 'info') {
      // @ts-ignore
      if (msg.title === 'Livestream Started') {

      } else {
        // @ts-ignore
        this.ngSnotify.info(msg.message, msg.title);
      }
    } else if (msg.type === 'error') {
      // @ts-ignore
      this.ngSnotify.error(msg.message, msg.title);

    } else if (msg.type === 'success') {
      // @ts-ignore
      if (msg.message.Request !== undefined) {
        // @ts-ignore
        if (msg.message.Request === 'AnalyzeImage') {
          // @ts-ignore
          this.analyzedImagesService.analyzedImages = this.analyzedImagesService.analyzedImages.concat(msg.message.Request.body);
        } else {
          // @ts-ignore
          this.analyzedVideosService.analyzeVideos = this.analyzedVideosService.analyzeVideos.concat(msg.message.Request.body);
        }
      } else {
        // @ts-ignore
        this.ngSnotify.success(msg.message, msg.title);

      }
    }
  }


}
