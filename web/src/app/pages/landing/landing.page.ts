import {AfterContentChecked, Component, OnInit, ViewChild, ViewEncapsulation} from '@angular/core';
import {SwiperComponent} from 'swiper/angular';
import {SwiperOptions} from 'swiper';
import SwiperCore, {Pagination, Mousewheel, Navigation, Autoplay, Keyboard} from 'swiper/core';

SwiperCore.use([Pagination, Mousewheel, Navigation, Autoplay, Keyboard]);

@Component({
  selector: 'app-landing',
  templateUrl: './landing.page.html',
  styleUrls: ['./landing.page.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LandingPage implements OnInit, AfterContentChecked {
  @ViewChild('swiper') swiper: SwiperComponent;
  // The config of the big swiper on the landing page, containing the landing page's content
  swiperConfig: SwiperOptions = {
    slidesPerView: 'auto',
    spaceBetween: 50,
    pagination: true,
    keyboard: true,
    navigation: true,
    speed: 800,
    loop: false,
    allowTouchMove: false,
  };

  constructor() {
    //Nothing added here yet
  }

  ngOnInit() {
    //Nothing added here yet
  }

  /**
   * Function needed to fix bug which caused swiper not to render properly
   */
  ngAfterContentChecked(): void {
    if (this.swiper) {
      this.swiper.updateSwiper({});
    }
  }
}
