import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { NavbarPage } from './navbar.page';
import {MsalGuard} from '../../services/msal-guard/msal-guard';

const routes: Routes = [
  {
    path: '',
    component: NavbarPage,
    children: [
      {
        path:'landing',
        loadChildren:() => import('../landing/landing.module').then(m=>m.LandingPageModule),
        canActivate: [MsalGuard]
      },
      {
        path:'analytics',
        loadChildren:() => import('../analytics/analytics.module').then(m => m.AnalyticsPageModule),
        canActivate: [MsalGuard]
      },
      {
        path: 'videos',
        loadChildren: () => import('../videostore/videostore.module').then( m => m.VideostorePageModule),
        canActivate: [MsalGuard]
      },
      {
        path:'',
        redirectTo : 'landing',
        pathMatch:'full',
        canActivate: [MsalGuard]
      },
      {
        path:'',
        redirectTo : 'landing',
        pathMatch:'full',
        canActivate: [MsalGuard]
      },
    ],
    canActivate: [MsalGuard]
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
