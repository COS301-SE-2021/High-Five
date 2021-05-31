import {Component, OnInit, ViewChild} from '@angular/core';

@Component({
  selector: 'app-videostream-card',
  templateUrl: './videostream-card.component.html',
  styleUrls: ['./videostream-card.component.scss'],
})
export class VideostreamCardComponent implements OnInit {
  @ViewChild('playMedia') playMedia : HTMLVideoElement;
  @ViewChild('videoPlay') vidPlay : HTMLIonButtonElement;
  @ViewChild('videoPause') vidPause : HTMLIonButtonElement;

  constructor() { }

  ngOnInit() {}

  playVideo() {
    console.log(this.vidPlay)
    this.vidPlay['el'].style.display = "none";
    this.vidPause['el'].style.display = "block";
    this.playMedia['nativeElement'].play(); // WHY do I need to access nativeElement before playing?
  }

  pauseVideo() {
    this.vidPlay['el'].style.display = "block";
    this.vidPause['el'].style.display = "none";
    this.playMedia['nativeElement'].pause(); // WHY do I need to access nativeElement before playing?
  }

}
