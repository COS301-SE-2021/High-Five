import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LandingPage } from './landing.page';
import {ControlsPage} from '../controls/controls.page';

const routes: Routes = [
  {
    path: '',
    component: LandingPage
  },
  {
    path:'landing',
    redirectTo: '',
    pathMatch: 'full'
  },
  {
    path: 'controls',
    component: ControlsPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class LandingPageRoutingModule {}
