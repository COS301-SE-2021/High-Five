import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { ApprovedToolsPageRoutingModule } from './approved-tools-routing.module';

import { ApprovedToolsPage } from './approved-tools.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ApprovedToolsPageRoutingModule
  ],
  declarations: [ApprovedToolsPage]
})
export class ApprovedToolsPageModule {}
