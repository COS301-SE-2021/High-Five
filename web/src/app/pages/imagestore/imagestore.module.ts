import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { ImagestorePageRoutingModule } from './imagestore-routing.module';

import { ImagestorePage } from './imagestore.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ImagestorePageRoutingModule
  ],
  declarations: [ImagestorePage]
})
export class ImagestorePageModule {}
