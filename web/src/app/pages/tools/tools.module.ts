import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';

import {IonicModule} from '@ionic/angular';

import {ToolsPageRoutingModule} from './tools-routing.module';

import {ToolsPage} from './tools.page';
import {AdminGuard} from '../../guards/admin.guard';
import {CreateToolComponent} from '../../components/create-tool/create-tool.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ToolsPageRoutingModule
  ],
  providers: [AdminGuard],
  declarations: [ToolsPage, CreateToolComponent]
})
export class ToolsPageModule {
}
