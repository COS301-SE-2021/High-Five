import {Component, OnInit} from '@angular/core';
import {AnimationOptions} from 'ngx-lottie';
import {LiveStreamingService} from '../../services/live-streaming/live-streaming.service';


@Component({
  selector: 'app-live',
  templateUrl: './live.page.html',
  styleUrls: ['./live.page.scss'],
})
export class LivePage implements OnInit {

  /**
   * The configuration of the lottie animation on this page (not present currently)
   */
  public lottieConfig: AnimationOptions = {
    path: '/assets/lottie-animations/67783-drones-isometric-lottie-animation.json'
  };

  constructor(public liveStreamingService: LiveStreamingService) {
  }



  ngOnInit() {
  }
}
