import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { LandingPageRoutingModule } from './landing-routing.module';

import { LandingPage } from './landing.page';
import {DesktopNavbarComponent} from '../../components/desktop-navbar/desktop-navbar.component';
import {MobileNavbarComponent} from '../../components/mobile-navbar/mobile-navbar.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    LandingPageRoutingModule
  ],
  declarations: [LandingPage, DesktopNavbarComponent, MobileNavbarComponent]
})
export class LandingPageModule {}
