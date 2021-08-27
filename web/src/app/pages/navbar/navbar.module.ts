import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';
import lottie from 'lottie-web';
import {IonicModule} from '@ionic/angular';

import {NavbarPageRoutingModule} from './navbar-routing.module';

import {NavbarPage} from './navbar.page';
import {defineLordIconElement} from 'lord-icon-element';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    NavbarPageRoutingModule
  ],
  declarations: [NavbarPage],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class NavbarPageModule {
  constructor() {
    defineLordIconElement(lottie.loadAnimation);
  }
}
