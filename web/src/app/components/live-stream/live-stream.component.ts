import {Component, Input, OnInit} from '@angular/core';
import {DomSanitizer, SafeResourceUrl} from '@angular/platform-browser';
import {LiveStreamingService} from '../../services/live-streaming/live-streaming.service';
import {environment} from '../../../environments/environment';

@Component({
  selector: 'app-live-stream',
  templateUrl: './live-stream.component.html',
  styleUrls: ['./live-stream.component.scss'],
})
export class LiveStreamComponent implements OnInit {
  @Input() streamId: string;

  // eslint-disable-next-line max-len

  constructor(private domSanitizer: DomSanitizer, private liveStreamingService: LiveStreamingService) {
  }

  ngOnInit() {

  }

// + '&token=' + this.getOTT()
  public getUrl(): SafeResourceUrl {
    const x: string = environment.streamPlayBaseUrl + this.liveStreamingService.appName + '/play.html?name=' + this.streamId;
    console.log(x);
    return this.domSanitizer.bypassSecurityTrustResourceUrl(x);
  }

  private getOTT(): string {
    return 'x';
  }

}
