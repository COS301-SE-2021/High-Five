import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { AllmediaPageRoutingModule } from './allmedia-routing.module';

import { AllmediaPage } from './allmedia.page';
import {CustomComponentsModule} from "../../components/components.module";

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    AllmediaPageRoutingModule,
    CustomComponentsModule
  ],
  declarations: [AllmediaPage]
})
export class AllmediaPageModule {}
