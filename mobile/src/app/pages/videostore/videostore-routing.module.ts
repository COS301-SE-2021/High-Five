import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { VideostorePage } from './videostore.page';

const routes: Routes = [
  {
    path: '',
    component: VideostorePage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class VideostorePageRoutingModule {}
