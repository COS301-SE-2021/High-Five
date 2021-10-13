import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';

import {UnapprovedToolsPage} from './unapproved-tools.page';
import {AdminGuard} from '../../guards/admin.guard';

const routes: Routes = [
  {
    path: '',
    component: UnapprovedToolsPage,
    canActivate: [AdminGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UnapprovedToolsPageRoutingModule {
}
