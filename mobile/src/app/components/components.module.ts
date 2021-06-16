import { NgModule } from '@angular/core';
import {CommonModule} from '@angular/common';
import {IonicModule} from '@ionic/angular';
import {RouterModule} from '@angular/router';
import {VideostoreCardComponent} from './videostore-card/videostore-card.component';
import {VideostreamCardComponent} from './videostream-card/videostream-card.component';
import {VgControlsModule} from '@videogular/ngx-videogular/controls';
import {VgCoreModule} from '@videogular/ngx-videogular/core';
import {PipelineComponent} from './pipeline/pipeline.component';

@NgModule({
  declarations: [VideostoreCardComponent, VideostreamCardComponent, PipelineComponent],
  imports: [
    CommonModule,
    IonicModule,
    RouterModule,
    VgControlsModule,
    VgCoreModule,
  ],
  exports: [VideostoreCardComponent, VideostreamCardComponent, PipelineComponent]
})
export class CustomComponentsModule {}
