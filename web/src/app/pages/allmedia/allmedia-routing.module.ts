import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AllmediaPage } from './allmedia.page';

const routes: Routes = [
  {
    path: '',
    component: AllmediaPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AllmediaPageRoutingModule {}
