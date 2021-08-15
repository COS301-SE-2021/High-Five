import {AfterContentChecked, Component, OnInit, ViewChild} from '@angular/core';
import {SwiperComponent} from "swiper/angular";
import {SwiperOptions} from "swiper";
import SwiperCore, {Autoplay, Mousewheel} from 'swiper/core';

SwiperCore.use([Mousewheel, Autoplay]);
@Component({
  selector: 'app-about-page3',
  templateUrl: './about-page3.component.html',
  styleUrls: ['./about-page3.component.scss'],
})
export class AboutPage3Component implements OnInit, AfterContentChecked {
  @ViewChild('swiper2') swiper2: SwiperComponent;
  swiperConfig2: SwiperOptions= {
    direction : "vertical",
    slidesPerView: 1,
    mousewheel: true,
    speed: 800,
    allowTouchMove: true,
    // autoplay: {
    //   delay: 12000,
    //   disableOnInteraction: true,
    // }
  }
  constructor() { }

  ngOnInit() {}

  ngAfterContentChecked(): void {
    if(this.swiper2){
      this.swiper2.updateSwiper({});
    }
  }

}
