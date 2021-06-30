import { Injectable } from '@angular/core';
import {BehaviorSubject, Observable} from 'rxjs';
import {distinctUntilChanged} from 'rxjs/operators';
import {Platform} from '@ionic/angular';

@Injectable({
  providedIn: 'root'
})
export class ScreenSizeServiceService {
  private isDesktop = new BehaviorSubject(false);
  constructor() { }

  onPlatformChange(platform: Platform){
    if (platform.is('desktop')){
      this.isDesktop.next(true);
    }else {
      this.isDesktop.next(false);
    }
  }

  onResize(size){
    if(size<600){
      this.isDesktop.next(false);
    }
  }

  isDesktopView(): Observable<boolean>{
    return this.isDesktop.asObservable().pipe(distinctUntilChanged());
  }
}
