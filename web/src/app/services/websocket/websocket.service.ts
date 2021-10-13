import {Injectable} from '@angular/core';
import {WebSocketSubject} from 'rxjs/internal-compatibility';
import {environment} from '../../../environments/environment';
import {webSocket} from 'rxjs/webSocket';
import {JsonObject} from '@angular/compiler-cli/ngcc/src/packages/entry_point';
import {SnotifyService} from 'ng-snotify';
import {AnalyzedVideosService} from '../analyzed-videos/analyzed-videos.service';
import {AnalyzedImagesService} from '../analyzed-images/analyzed-images.service';
import {LiveStreamingService} from '../live-streaming/live-streaming.service';
import {OAuthService} from 'angular-oauth2-oidc';


@Injectable({
  providedIn: 'root'
})
export class WebsocketService {
  private socket: WebSocketSubject<any>;


  constructor(private ngSnotify: SnotifyService, private analyzedVideosService: AnalyzedVideosService,
              private analyzedImagesService: AnalyzedImagesService, private liveStreamingService: LiveStreamingService,
              private oauthService: OAuthService) {
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
      this.handleMessage(msg);
    });
    this.sendMessage({
      // eslint-disable-next-line @typescript-eslint/naming-convention
      Request: 'Synchronize'
    });
  }

  public sendMessage(message: JsonObject) {
    message.Authorization = this.oauthService.getAccessToken();
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
    this.ngSnotify.info('Request sent to analyze image');
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
    this.ngSnotify.info('Request sent to analyze video');
    this.sendMessage({
      // eslint-disable-next-line @typescript-eslint/naming-convention
      Request: 'AnalyzeVideo',
      // eslint-disable-next-line @typescript-eslint/naming-convention
      Body: {
        videoId: mediaId,
        pipelineId,
      },
    });
  }

  private handleMessage(msg: JsonObject) {
    if (msg.type === 'info') {
      // @ts-ignore
      if (msg.title === 'Livestream Started') {
        this.ngSnotify.info('Live Stream Started, refresh the live page', 'Live Stream Started');
      } else {
        // @ts-ignore
        this.ngSnotify.info(msg.message, msg.title);
      }
    } else if (msg.type === 'error') {
      // @ts-ignore
      this.ngSnotify.error(msg.message, msg.title);

    } else if (msg.type === 'success') {
      // @ts-ignore
      // @ts-ignore
      if (msg.title === 'Image Analysed') {
        // @ts-ignore
        this.analyzedImagesService.analyzedImages = this.analyzedImagesService.analyzedImages.concat(msg.message);
        // @ts-ignore
        this.ngSnotify.success('Analyzed Image', msg.title);
      } else if (msg.title === 'Video Analysed') {
        // @ts-ignore
        this.analyzedVideosService.analyzeVideos = this.analyzedVideosService.analyzeVideos.concat(msg.message);
        this.ngSnotify.success('Analyzed video', msg.title);
      } else {
        // @ts-ignore
        this.ngSnotify.success(msg.message, msg.title);
      }
    } else {
      // @ts-ignore
      this.ngSnotify.success(msg.message, msg.title);

    }
  }


}
