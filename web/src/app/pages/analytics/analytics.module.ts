import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';

import {IonicModule} from '@ionic/angular';

import {AnalyticsPageRoutingModule} from './analytics-routing.module';

import {AnalyticsPage} from './analytics.page';
import {SwiperModule} from 'swiper/angular';
import {AddPipelineComponent} from '../../components/add-pipeline/add-pipeline.component';
import {CustomComponentsModule} from "../../components/components.module";

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    AnalyticsPageRoutingModule,
    SwiperModule,
    CustomComponentsModule
  ],
  declarations: [AnalyticsPage],
  entryComponents: [AddPipelineComponent]
})
export class AnalyticsPageModule {
}
