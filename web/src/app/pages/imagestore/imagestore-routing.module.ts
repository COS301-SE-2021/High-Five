import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ImagestorePage } from './imagestore.page';

const routes: Routes = [
  {
    path: '',
    component: ImagestorePage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ImagestorePageRoutingModule {}
