import {Component, Input, OnInit} from '@angular/core';
import {DomSanitizer, SafeResourceUrl} from '@angular/platform-browser';

@Component({
  selector: 'app-live-stream',
  templateUrl: './live-stream.component.html',
  styleUrls: ['./live-stream.component.scss'],
})
export class LiveStreamComponent implements OnInit {
  @Input() streamId: string;
  // eslint-disable-next-line max-len

  constructor(private domSanitizer: DomSanitizer) {
  }

  ngOnInit() {

  }

  public getUrl(): SafeResourceUrl {
    const x: string = 'https://highfiveanalysis.ddns.net:5443/test5/play.html?name=' + this.streamId + '&token=' + this.getOTT();
    console.log(x);
    return this.domSanitizer.bypassSecurityTrustResourceUrl(x);
  }

  private getOTT(): string {
    return 'x';
  }

}
