import {Component, OnInit, ViewChild} from '@angular/core';

@Component({
  selector: 'app-videostream-card',
  templateUrl: './videostream-card.component.html',
  styleUrls: ['./videostream-card.component.scss'],
})
export class VideostreamCardComponent implements OnInit {
  @ViewChild('playMedia') playMedia : HTMLVideoElement

  constructor() { }

  isPaused = false;

  ngOnInit() {}

  playVideo() {
    this.isPaused = false;
    this.playMedia['nativeElement'].play(); // WHY do I need to access nativeElement before playing?
  }

  pauseVideo() {
    this.isPaused = true;
    this.playMedia['nativeElement'].pause(); // WHY do I need to access nativeElement before playing?
  }

}
