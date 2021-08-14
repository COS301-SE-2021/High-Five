import {AfterContentChecked, Component, OnInit, ViewChild, ViewEncapsulation} from '@angular/core';
import {SwiperComponent} from "swiper/angular";
import {SwiperOptions} from "swiper";
import SwiperCore, {Pagination} from 'swiper/core';

SwiperCore.use([Pagination]);
@Component({
  selector: 'app-landing',
  templateUrl: './landing.page.html',
  styleUrls: ['./landing.page.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LandingPage implements OnInit, AfterContentChecked {
  @ViewChild('swiper') swiper: SwiperComponent;
  swiperConfig: SwiperOptions= {
    slidesPerView: 2,
    spaceBetween: 50,
    pagination: true
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
}
