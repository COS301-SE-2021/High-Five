import {Inject, Injectable, Renderer2, RendererFactory2} from '@angular/core';
import {DOCUMENT} from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {

  private renderer: Renderer2;
  private darkMode: boolean;
  constructor(private rendererFactory: RendererFactory2,@Inject(DOCUMENT) private document: Document ) {
    this.renderer= this.rendererFactory.createRenderer(null,null);
    this.darkMode= false;
  }

  enableDarkMode(){
    this.renderer.addClass(this.document.body,'dark');
    this.renderer.removeClass(this.document.body,'light');
    this.darkMode= true;
  }

  disableDarkMode(){
    this.renderer.removeClass(this.document.body,'dark');
    this.renderer.addClass(this.document.body,'light');
    this.darkMode= false;
  }

  toggleMode(){
    if (this.darkMode){
      this.disableDarkMode();
    }else{
      this.enableDarkMode();
    }
    console.log('Is current mode dark : ', this.darkMode);
  }

  isDarkMode(){
    return this.darkMode;
  }





}
