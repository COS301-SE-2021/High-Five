import {NgModule} from '@angular/core';
import {PreloadAllModules, RouterModule, Routes} from '@angular/router';
import {MsalGuard} from '@azure/msal-angular';

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
    canLoad: [MsalGuard]
  },
  {
    path: 'media',
    loadChildren: () => import('./pages/media/media.module').then( m => m.MediaPageModule)
  },


];
const isIframe = window !== window.parent && !window.opener;

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {preloadingStrategy: PreloadAllModules, initialNavigation: !isIframe ? 'enabled' : 'disabled'})
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
