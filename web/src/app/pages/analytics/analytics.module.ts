import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { AnalyticsPageRoutingModule } from './analytics-routing.module';

import { AnalyticsPage } from './analytics.page';
import {CustomComponentsModule} from '../../components/components.module';
import {SwiperModule} from 'swiper/angular';
import {EditPipelineComponent} from '../../components/edit-pipeline/edit-pipeline.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    AnalyticsPageRoutingModule,
    CustomComponentsModule,
    SwiperModule
  ],
  declarations: [AnalyticsPage,EditPipelineComponent],
  entryComponents: [EditPipelineComponent]
})
export class AnalyticsPageModule {}
