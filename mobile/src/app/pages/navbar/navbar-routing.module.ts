import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { NavbarPage } from './navbar.page';

const routes: Routes = [
  {
    path: '',
    component: NavbarPage,
    children: [
      {
        path:'landing',
        loadChildren:() => import('../landing/landing.module').then(m=>m.LandingPageModule)
      },
      {
        path:'controls',
        loadChildren:() => import('../controls/controls.module').then(m=>m.ControlsPageModule)
      },
      {
        path:'analytics',
        loadChildren:() => import('../analytics/analytics.module').then(m => m.AnalyticsPageModule)
      },
      {
        path: 'videos',
        loadChildren: () => import('../videostore/videostore.module').then( m => m.VideostorePageModule)
      },
      {
        path:'',
        redirectTo : 'landing',
        pathMatch:'full'
      },
      {
        path:'',
        redirectTo : 'landing',
        pathMatch:'full'
      },
    ]
  },
  {
    path:'',
    redirectTo:'navbar/landing',
    pathMatch:'full'
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class NavbarPageRoutingModule {}
