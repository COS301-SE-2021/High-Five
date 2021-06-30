import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ControlsPage } from './controls.page';

const routes: Routes = [
  {
    path: '',
    component: ControlsPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ControlsPageRoutingModule {}
