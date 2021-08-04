import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';

import {LandingPage} from './landing.page';
import {MsalGuard} from '../../services/msal-guard/msal-guard';

const routes: Routes = [
  {
    path: '',
    component: LandingPage,
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class LandingPageRoutingModule {
}
