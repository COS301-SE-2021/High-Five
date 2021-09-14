import {Component, OnInit} from '@angular/core';
import {AnimationOptions} from 'ngx-lottie';


@Component({
  selector: 'app-live',
  templateUrl: './live.page.html',
  styleUrls: ['./live.page.scss'],
})
export class LivePage implements OnInit {
  public liveStreams = [];

  /**
   * The configuration of the lottie animation on this page (not present currently)
   */
  public lottieConfig: AnimationOptions = {
    path: '/assets/lottie-animations/67783-drones-isometric-lottie-animation.json'
  };

  constructor() {
    for (let i = 0; i < 1; i++) {
      this.liveStreams = this.liveStreams.concat([{
        title: 'Test',
      }]);
    }
  }


  ngOnInit() {
  }
}
