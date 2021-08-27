import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {IonicModule} from '@ionic/angular';
import {RouterModule} from '@angular/router';
import {VideostoreCardComponent} from './videostore-card/videostore-card.component';
import {VideostreamCardComponent} from './videostream-card/videostream-card.component';
import {VgControlsModule} from '@videogular/ngx-videogular/controls';
import {VgCoreModule} from '@videogular/ngx-videogular/core';
import {FormsModule} from '@angular/forms';
import {AboutPage1Component} from './about-page1/about-page1.component';
import {AboutPage2Component} from './about-page2/about-page2.component';
import {AboutPage3Component} from './about-page3/about-page3.component';
import {AddItemComponent} from './add-item/add-item.component';
import {AddPipelineComponent} from './add-pipeline/add-pipeline.component';
import {AnalyzedImageCardComponent} from './analyzed-image-card/analyzed-image-card.component';
import {AnalyzedVideostoreCardComponent} from './analyzed-videostore-card/analyzed-videostore-card.component';
import {ImageCardComponent} from './image-card/image-card.component';
import {MoreInfoComponent} from './more-info/more-info.component';
import {NavbarMediaPopoverComponent} from './navbar-media-popover/navbar-media-popover.component';
import {PipelineComponent} from './pipeline/pipeline.component';
import {RegisterCardComponent} from './register-card/register-card.component';
import {WelcomeCardComponent} from './welcome-card/welcome-card.component';
import {LottieModule} from 'ngx-lottie';
import {SwiperModule} from 'swiper/angular';
import {MediaFilterComponent} from './media-filter/media-filter.component';
import {AccountComponent} from './account/account.component';
import {AccountPopoverComponent} from './account-popover/account-popover.component';
import {defineLordIconElement} from 'lord-icon-element';
import lottie from 'lottie-web';

@NgModule({
  declarations: [VideostoreCardComponent, VideostreamCardComponent, AboutPage1Component, AboutPage2Component,
    AboutPage3Component, AddItemComponent, AddPipelineComponent, AnalyzedImageCardComponent,
    AnalyzedVideostoreCardComponent, ImageCardComponent, MoreInfoComponent, NavbarMediaPopoverComponent,
    PipelineComponent, RegisterCardComponent, WelcomeCardComponent, MediaFilterComponent, AccountComponent,
    AccountPopoverComponent],
  imports: [
    CommonModule,
    IonicModule,
    RouterModule,
    VgControlsModule,
    VgCoreModule,
    FormsModule,
    LottieModule,
    SwiperModule,
  ],
  exports: [VideostoreCardComponent, VideostreamCardComponent, AboutPage1Component, AboutPage2Component,
    AboutPage3Component, AddItemComponent, AddPipelineComponent, AnalyzedImageCardComponent,
    AnalyzedVideostoreCardComponent, ImageCardComponent, MoreInfoComponent, NavbarMediaPopoverComponent,
    PipelineComponent, RegisterCardComponent, WelcomeCardComponent, MediaFilterComponent, AccountComponent,
    AccountPopoverComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class CustomComponentsModule {
  constructor() {
    defineLordIconElement(lottie.loadAnimation);
  }
}
