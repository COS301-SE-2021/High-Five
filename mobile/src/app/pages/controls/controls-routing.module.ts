import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ControlsPage } from './controls.page';
import {LandingPage} from '../landing/landing.page';

const routes: Routes = [
  {
    path: '',
    component: ControlsPage
  },
  {
    path:'landing',
    component:LandingPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ControlsPageRoutingModule {}
