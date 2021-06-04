import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { VideostorePageRoutingModule } from './videostore-routing.module';

import { VideostorePage } from './videostore.page';
import {CustomComponentsModule} from "../../components/components.module";

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    VideostorePageRoutingModule,
    CustomComponentsModule
  ],
  declarations: [VideostorePage]
})
export class VideostorePageModule {}
