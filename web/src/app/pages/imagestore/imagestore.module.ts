import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { ImagestorePageRoutingModule } from './imagestore-routing.module';

import { ImagestorePage } from './imagestore.page';
import {VideostorePageModule} from "../videostore/videostore.module";

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ImagestorePageRoutingModule,
    VideostorePageModule
  ],
  declarations: [ImagestorePage]
})
export class ImagestorePageModule {}
