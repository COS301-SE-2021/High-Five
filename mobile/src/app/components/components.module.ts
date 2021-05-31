import { NgModule } from '@angular/core';
import {CommonModule} from '@angular/common';
import {IonicModule} from '@ionic/angular';
import {RouterModule} from '@angular/router';
import {VideostoreCardComponent} from "./videostore-card/videostore-card.component";

@NgModule({
  declarations: [VideostoreCardComponent],
  imports: [
    CommonModule,
    IonicModule,
    RouterModule
  ],
  exports: [VideostoreCardComponent]
})
export class CustomComponentsModule {}
