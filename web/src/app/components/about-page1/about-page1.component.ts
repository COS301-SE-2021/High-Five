import {Component, OnInit} from '@angular/core';
import {AnimationOptions} from 'ngx-lottie';

@Component({
  selector: 'app-about-page1',
  templateUrl: './about-page1.component.html',
  styleUrls: ['./about-page1.component.scss'],
})
export class AboutPage1Component implements OnInit {


  /**
   * The below AnimationOptions are the lottie configurations used by the ng-lottie components in html
   */
  lottieConfig: AnimationOptions = {
    path: '/assets/lottie-animations/67783-drones-isometric-lottie-animation.json'
  };
  lottieConfig2: AnimationOptions = {
    path: '/assets/lottie-animations/72680-mobile-app.json',
    loop: false

  };
  lottieConfigArrowRight: AnimationOptions = {
    path: '/assets/lottie-animations/71249-arrow-pointing-to-right.json',
    loop: false

  };
  lottieConfig3: AnimationOptions = {
    path: '/assets/lottie-animations/41703-cloud-server.json',

  };

  constructor() {
    // Nothing added here
  }

  ngOnInit() {
  }

}
