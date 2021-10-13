import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';

import {IonicModule} from '@ionic/angular';

import {UnapprovedToolsPageRoutingModule} from './unapproved-tools-routing.module';

import {UnapprovedToolsPage} from './unapproved-tools.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    UnapprovedToolsPageRoutingModule
  ],
  declarations: [UnapprovedToolsPage]
})
export class UnapprovedToolsPageModule {
}
