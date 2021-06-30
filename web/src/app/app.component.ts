import {Component, HostListener} from '@angular/core';
import {Platform} from '@ionic/angular';
import {ScreenSizeServiceService} from './services/screen-size-service.service';
import {ThemeService} from './services/theme/theme.service';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent {
  constructor(private platform: Platform, private screensizeservice: ScreenSizeServiceService, private themeService: ThemeService) {
    this.initializeApp();
  }

  //TODO : Look into better way to change platform, other than resizing
  //Source for idea : https://youtu.be/FVwuCO5vJxI
  @HostListener('window:resize',['$event'])
  private onResize(event){
      this.screensizeservice.onResize(event.target.innerWidth);
    }

    initializeApp(){
      this.platform.ready().then(()=>{
      this.screensizeservice.onPlatformChange(this.platform);
      this.themeService.enableDarkMode();
    });
  }
}
