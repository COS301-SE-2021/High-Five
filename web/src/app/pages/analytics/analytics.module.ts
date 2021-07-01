import {NgModule} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { AnalyticsPageRoutingModule } from './analytics-routing.module';

import { AnalyticsPage } from './analytics.page';
import {SwiperModule} from 'swiper/angular';
import {PipelineComponent} from '../../components/pipeline/pipeline.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    AnalyticsPageRoutingModule,
    SwiperModule,],
    declarations: [AnalyticsPage, PipelineComponent]
})
export class AnalyticsPageModule {}
