import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';

import {IonicModule} from '@ionic/angular';

import {LandingPageRoutingModule} from './landing-routing.module';
import {LandingPage} from './landing.page';
import {SwiperModule} from "swiper/angular";
import {LottieModule} from "ngx-lottie";
import player from 'lottie-web';

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
  declarations: [LandingPage]
})
export class LandingPageModule {
}
