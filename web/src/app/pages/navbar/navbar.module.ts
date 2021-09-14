import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';
import lottie from 'lottie-web';
import {IonicModule} from '@ionic/angular';

import {NavbarPageRoutingModule} from './navbar-routing.module';

import {NavbarPage} from './navbar.page';
import {defineLordIconElement} from 'lord-icon-element';
import {UsersService} from '../../services/users/users.service';
import {WebsocketService} from '../../services/websocket/websocket.service';
import {CreateToolComponent} from '../../components/create-tool/create-tool.component';


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    NavbarPageRoutingModule
  ],
  providers: [WebsocketService],
  declarations: [NavbarPage, CreateToolComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class NavbarPageModule {
  constructor(private usersService: UsersService) {
    defineLordIconElement(lottie.loadAnimation);

  }
}
