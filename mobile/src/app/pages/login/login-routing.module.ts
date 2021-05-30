import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LoginPage } from './login.page';
import {WelcomePage} from '../welcome/welcome.page';
import {LandingPage} from '../landing/landing.page';

const routes: Routes = [
  {
    path: '',
    component: LoginPage
  },
  {
    path: 'welcome',
    component: WelcomePage
  },
  {
    path: 'landing',
    component: LandingPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class LoginPageRoutingModule {}
