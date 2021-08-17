import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { VideostorePageRoutingModule } from './videostore-routing.module';

import { VideostorePage } from './videostore.page';
import {CustomComponentsModule} from '../../components/components.module';
import {ImageCardComponent} from '../../components/image-card/image-card.component';
import {AnalyzedVideostoreCardComponent} from "../../components/analyzed-videostore-card/analyzed-videostore-card.component";
import {AnalyzedImageCardComponent} from "../../components/analyzed-image-card/analyzed-image-card.component";

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    VideostorePageRoutingModule,
    CustomComponentsModule
  ],
  declarations: [VideostorePage, ImageCardComponent, AnalyzedVideostoreCardComponent, AnalyzedImageCardComponent]
})
export class VideostorePageModule {}
