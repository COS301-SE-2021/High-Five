import {AfterContentChecked, Component, OnInit, ViewChild} from '@angular/core';
import {SwiperComponent} from 'swiper/angular';
import {SwiperOptions} from 'swiper';
import SwiperCore, {Autoplay, Mousewheel} from 'swiper/core';
import {AnimationOptions} from 'ngx-lottie';

SwiperCore.use([Mousewheel, Autoplay]);

@Component({
  selector: 'app-about-page3',
  templateUrl: './about-page3.component.html',
  styleUrls: ['./about-page3.component.scss'],
})
export class AboutPage3Component implements OnInit, AfterContentChecked {
  @ViewChild('swiper2') swiper2: SwiperComponent;
  swiperConfig2: SwiperOptions = {
    direction: 'vertical',
    slidesPerView: 1,
    mousewheel: true,
    speed: 800,
    allowTouchMove: true,
  };

  lottieScrollDownConfig: AnimationOptions = {
    path: '/assets/lottie-animations/scroll-down-animation.json'
  };
  lottieServerConfig: AnimationOptions = {
    path: '/assets/lottie-animations/41703-cloud-server.json'
  };
  lottieExplainConfig: AnimationOptions = {
    path: '/assets/lottie-animations/explain.json'
  };

  constructor() {
  }

  ngOnInit() {
  }

  ngAfterContentChecked(): void {
    if (this.swiper2) {
      this.swiper2.updateSwiper({});
    }
  }

}
