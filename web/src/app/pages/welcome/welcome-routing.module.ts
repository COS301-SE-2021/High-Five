import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { WelcomePage } from './welcome.page';
import {LoginPage} from '../login/login.page';
import {RegisterPage} from '../register/register.page';

const routes: Routes = [
  {
    path: '',
    component: WelcomePage
  },
  {
    path:'login',
    component: LoginPage
  },
  {
    path:'register',
    component: RegisterPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class WelcomePageRoutingModule {}
