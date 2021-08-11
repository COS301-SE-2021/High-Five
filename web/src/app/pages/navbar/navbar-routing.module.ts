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
        path: 'videos',
        loadChildren: () => import('../videostore/videostore.module').then( m => m.VideostorePageModule),
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
