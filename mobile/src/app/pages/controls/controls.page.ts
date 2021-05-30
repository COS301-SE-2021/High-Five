import { Component, OnInit } from '@angular/core';
import {ScreenSizeServiceService} from '../../services/screen-size-service.service';

@Component({
  selector: 'app-controls',
  templateUrl: './controls.page.html',
  styleUrls: ['./controls.page.scss'],
})
export class ControlsPage implements OnInit {
  isDesktop: boolean;
  constructor(private screenSizeService: ScreenSizeServiceService) {
    this.screenSizeService.isDesktopView().subscribe(isDesktop=>{
      this.isDesktop = isDesktop;
    });
  }

  ngOnInit() {
  }

}
