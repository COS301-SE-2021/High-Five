import {AfterContentChecked, Component, OnInit, ViewChild, ViewEncapsulation} from '@angular/core';
import {SwiperComponent} from "swiper/angular";
import {SwiperOptions} from "swiper";
import SwiperCore, {Pagination, Mousewheel, Navigation, Autoplay} from 'swiper/core';
import {SwiperEvents} from "swiper/types";

SwiperCore.use([Pagination, Mousewheel, Navigation, Autoplay]);
@Component({
  selector: 'app-landing',
  templateUrl: './landing.page.html',
  styleUrls: ['./landing.page.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LandingPage implements OnInit, AfterContentChecked {
  @ViewChild('swiper') swiper: SwiperComponent;
  swiperConfig: SwiperOptions= {
    slidesPerView: "auto",
    spaceBetween: 50,
    pagination: true,
    mousewheel: true,
    navigation: true,
    speed: 800,
    loop: true,
    allowTouchMove: false,
    autoplay: {
      delay: 6000,
      disableOnInteraction: true,
    }
  }

  constructor() {
    //Nothing added here yet
  }

  ngOnInit() {
    //Nothing added here yet
  }

  ngAfterContentChecked(): void {
    if(this.swiper){
      this.swiper.updateSwiper({});
    }
  }

  swiperSlideChanged(event: SwiperEvents["slideChange"]) {
    console.log(event);
  }
}
