import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';

import {NavbarPage} from './navbar.page';
import {AuthGuard} from '../../guards/auth.guard';

const routes: Routes = [
  {
    path: '',
    component: NavbarPage,
    canLoad: [AuthGuard],
    children: [
      {
        path: 'landing',
        loadChildren: () => import('../landing/landing.module').then(m => m.LandingPageModule),
      },
      {
        path: 'pipelines',
        loadChildren: () => import('../analytics/analytics.module').then(m => m.AnalyticsPageModule),
      },
      {
        path: 'media',
        loadChildren: () => import('../media/media.module').then(m => m.MediaPageModule),
      },
      {
        path: 'live',
        loadChildren: () => import('../live/live.module').then(m => m.LivePageModule),
      },
      {
        path: 'tools',
        loadChildren: () => import('../tools/tools.module').then(m => m.ToolsPageModule)
      },
      {
        path: '',
        redirectTo: 'landing',
        pathMatch: 'full',
        canActivate: [AuthGuard]

      },
    ]
  },
  {
    path: '',
    redirectTo: 'navbar/landing',
    pathMatch: 'full',
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class NavbarPageRoutingModule {
}
