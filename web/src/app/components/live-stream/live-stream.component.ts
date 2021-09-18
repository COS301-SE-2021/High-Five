import {Component, Input, OnInit} from '@angular/core';
import {DomSanitizer} from '@angular/platform-browser';
import {LiveStreamingService} from '../../services/live-streaming/live-streaming.service';
import {environment} from '../../../environments/environment';
import {LiveStream} from '../../models/liveStream';
import {Platform} from '@ionic/angular';

@Component({
  selector: 'app-live-stream',
  templateUrl: './live-stream.component.html',
  styleUrls: ['./live-stream.component.scss'],
})
export class LiveStreamComponent implements OnInit {
  @Input() stream: LiveStream;
  public url: string;

  // eslint-disable-next-line max-len

  constructor(public domSanitizer: DomSanitizer, private liveStreamingService: LiveStreamingService,
              private platform: Platform) {


  }

  ngOnInit() {
    this.platform.ready().then(() => {
      this.url = environment.streamPlayBaseUrl +
        this.liveStreamingService.appName + '/play.html?name=' + this.stream.streamId;
    });
  }

}
