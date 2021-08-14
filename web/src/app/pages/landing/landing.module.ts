import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';

import {IonicModule} from '@ionic/angular';

import {LandingPageRoutingModule} from './landing-routing.module';
import {LandingPage} from './landing.page';
import {SwiperModule} from "swiper/angular";
import {LottieModule} from "ngx-lottie";
import player from 'lottie-web';
import {AboutPage1Component} from "../../components/about-page1/about-page1.component";
import {AboutPage2Component} from "../../components/about-page2/about-page2.component";
export function playerFactory() {
  return player;
}

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    LandingPageRoutingModule,
    SwiperModule,
    LottieModule.forRoot({player: playerFactory})
  ],
  declarations: [LandingPage, AboutPage1Component, AboutPage2Component]
})
export class LandingPageModule {
}
