import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';

import {IonicModule} from '@ionic/angular';

import {LivePageRoutingModule} from './live-routing.module';

import {LivePage} from './live.page';
import {CustomComponentsModule} from '../../components/components.module';
import {LiveStreamComponent} from '../../components/live-stream/live-stream.component';
import {LiveAnalysisFilterComponent} from '../../components/live-analysis-filter/live-analysis-filter.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    LivePageRoutingModule,
    CustomComponentsModule,
  ],
  declarations: [LivePage, LiveStreamComponent, LiveAnalysisFilterComponent]
})
export class LivePageModule {
}
