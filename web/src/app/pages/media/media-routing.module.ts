import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';

import {MediaPage} from './media.page';
import {AuthGuard} from '../../guards/auth.guard';

const routes: Routes = [
  {
    path: '',
    component: MediaPage,
    canActivateChild: [AuthGuard],
    children: [
      {
        path: 'videos',
        loadChildren: () => import('../videostore/videostore.module').then(m => m.VideostorePageModule),
      },
      {
        path: 'images',
        loadChildren: () => import('../imagestore/imagestore.module').then(m => m.ImagestorePageModule)
      },
      {
        path: 'all',
        loadChildren: () => import('../allmedia/allmedia.module').then(m => m.AllmediaPageModule)
      },
      {
        path: 'live',
        loadChildren: () => import('../live/live.module').then(m => m.LivePageModule)
      },
      {
        path: '',
        redirectTo: 'all',
        pathMatch: 'full',
        canActivate: [AuthGuard]
      }
    ],
  },
  {
    path: '',
    redirectTo: 'media/all',
    pathMatch: 'full',
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MediaPageRoutingModule {
}
