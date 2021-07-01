import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { VideostorePageRoutingModule } from './videostore-routing.module';

import { VideostorePage } from './videostore.page';
import {VideostoreCardComponent} from '../../components/videostore-card/videostore-card.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    VideostorePageRoutingModule
  ],
  declarations: [VideostorePage, VideostoreCardComponent]
})
export class VideostorePageModule {}
