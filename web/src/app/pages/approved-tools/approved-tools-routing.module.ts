import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ApprovedToolsPage } from './approved-tools.page';

const routes: Routes = [
  {
    path: '',
    component: ApprovedToolsPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ApprovedToolsPageRoutingModule {}
