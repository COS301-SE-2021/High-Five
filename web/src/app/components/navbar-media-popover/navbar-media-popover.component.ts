import {Component, Input, OnInit} from '@angular/core';
import {Router} from "@angular/router";
import {PopoverController} from "@ionic/angular";

@Component({
  selector: 'app-navbar-media-popover',
  templateUrl: './navbar-media-popover.component.html',
  styleUrls: ['./navbar-media-popover.component.scss'],
})
export class NavbarMediaPopoverComponent implements OnInit {
  constructor(private router: Router) {
  }

  ngOnInit() {
  }

  navigateAll() {
    this.router.navigate(['/navbar/all']).then(() => {

    });
  }

  navigateImages() {
    this.router.navigate(['/navbar/images']).then(() => {

    });
  }

  navigateVideos() {
    this.router.navigate(['/navbar/videos']).then(()=>{

    });
  }

}
