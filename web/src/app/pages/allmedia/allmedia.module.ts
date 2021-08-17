import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { AllmediaPageRoutingModule } from './allmedia-routing.module';

import { AllmediaPage } from './allmedia.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    AllmediaPageRoutingModule
  ],
  declarations: [AllmediaPage]
})
export class AllmediaPageModule {}
