import {Component, Input, OnInit} from '@angular/core';
import {DomSanitizer, SafeResourceUrl} from '@angular/platform-browser';
import {LiveStreamingService} from '../../services/live-streaming/live-streaming.service';
import {environment} from '../../../environments/environment';
import {LiveStream} from "../../models/liveStream";

@Component({
  selector: 'app-live-stream',
  templateUrl: './live-stream.component.html',
  styleUrls: ['./live-stream.component.scss'],
})
export class LiveStreamComponent implements OnInit {
  @Input() stream: LiveStream;

  // eslint-disable-next-line max-len

  constructor(private domSanitizer: DomSanitizer, private liveStreamingService: LiveStreamingService) {
  }

  ngOnInit() {

  }

// + '&token=' + this.getOTT()
  public getUrl(): SafeResourceUrl {
    const x: string = environment.streamPlayBaseUrl + this.liveStreamingService.appName + '/play.html?name=' + this.stream.streamId+
    '&token='+ this.stream.oneTimeToken;
    this.liveStreamingService.getStreamToken(this.stream.streamId);
    return this.domSanitizer.bypassSecurityTrustResourceUrl(x);
  }

  private getOTT(): string {
    return 'x';
  }

}
