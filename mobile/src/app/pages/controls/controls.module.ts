import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { ControlsPageRoutingModule } from './controls-routing.module';

import { ControlsPage } from './controls.page';
import {DesktopNavbarComponent} from '../../components/desktop-navbar/desktop-navbar.component';
import {MobileNavbarComponent} from '../../components/mobile-navbar/mobile-navbar.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ControlsPageRoutingModule
  ],
  declarations: [ControlsPage, DesktopNavbarComponent, MobileNavbarComponent]
})
export class ControlsPageModule {}
