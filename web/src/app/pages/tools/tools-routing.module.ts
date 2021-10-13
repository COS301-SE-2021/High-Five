import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';

import {ToolsPage} from './tools.page';
import {AuthGuard} from '../../guards/auth.guard';

const routes: Routes = [
  {
    path: '',
    component: ToolsPage,
    canActivate: [AuthGuard],
    children: [
      {
        path: 'approved',
        loadChildren: () => import('../approved-tools/approved-tools.module').then(m => m.ApprovedToolsPageModule)
      },
      {
        path: 'unapproved',
        loadChildren: () => import('../unapproved-tools/unapproved-tools.module').then(m => m.UnapprovedToolsPageModule),
      },
      {
        path: '',
        redirectTo: 'approved',
        pathMatch: 'full',
        canActivate: [AuthGuard]
      }
    ]
  },
  {
    path: '',
    redirectTo: 'tools/approved',
    pathMatch: 'full',
    canActivate: [AuthGuard]
  }


];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ToolsPageRoutingModule {
}
