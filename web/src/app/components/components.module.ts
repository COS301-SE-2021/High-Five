import { NgModule } from '@angular/core';
import {CommonModule} from '@angular/common';
import {IonicModule} from '@ionic/angular';
import {RouterModule} from '@angular/router';
import {VideostoreCardComponent} from './videostore-card/videostore-card.component';
import {VideostreamCardComponent} from './videostream-card/videostream-card.component';
import {VgControlsModule} from '@videogular/ngx-videogular/controls';
import {VgCoreModule} from '@videogular/ngx-videogular/core';
import {FormsModule} from '@angular/forms';

@NgModule({
  declarations: [VideostoreCardComponent, VideostreamCardComponent],
    imports: [
        CommonModule,
        IonicModule,
        RouterModule,
        VgControlsModule,
        VgCoreModule,
        FormsModule,
    ],
  exports: [VideostoreCardComponent, VideostreamCardComponent]
})
export class CustomComponentsModule {}
