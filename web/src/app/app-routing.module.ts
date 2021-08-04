import {NgModule} from '@angular/core';
import {PreloadAllModules, RouterModule, Routes} from '@angular/router';
import {MsalGuard} from './services/msal-guard/msal-guard';

const routes: Routes = [

  {
    path: '',
    redirectTo: 'welcome',
    pathMatch: 'full'
  },
  {
    path: 'welcome',
    loadChildren: () => import('./pages/welcome/welcome.module').then(m => m.WelcomePageModule)
  },
  {

    path: 'navbar',
    loadChildren: () => import('./pages/navbar/navbar.module').then(m => m.NavbarPageModule),
    canActivate: [MsalGuard]
  },

];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {preloadingStrategy: PreloadAllModules})
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
