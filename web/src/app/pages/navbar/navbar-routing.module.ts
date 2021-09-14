import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { NavbarPage } from './navbar.page';
import {MsalGuard} from '@azure/msal-angular';

const routes: Routes = [
  {
    path: '',
    component: NavbarPage,
    canActivateChild : [MsalGuard],
    children: [
      {
        path:'landing',
        loadChildren:() => import('../landing/landing.module').then(m=>m.LandingPageModule),
      },
      {
        path:'analytics',
        loadChildren:() => import('../analytics/analytics.module').then(m => m.AnalyticsPageModule),
      },
      {
        path: 'media',
        loadChildren:() => import('../media/media.module').then(m => m.MediaPageModule),
      },
      {
        path:'live',
        loadChildren:() => import('../live/live.module').then(m => m.LivePageModule),
      },
      {
        path:'',
        redirectTo : 'landing',
        pathMatch:'full',
        canActivate: [MsalGuard]
      },
    ]
  },
  {
    path:'',
    redirectTo:'navbar/landing',
    pathMatch:'full',
    canActivate: [MsalGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class NavbarPageRoutingModule {}
