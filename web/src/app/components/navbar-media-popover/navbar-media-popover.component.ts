import {Component, Input, OnInit, Output} from '@angular/core';
import {Router} from "@angular/router";
import {PopoverController} from "@ionic/angular";

@Component({
  selector: 'app-navbar-media-popover',
  templateUrl: './navbar-media-popover.component.html',
  styleUrls: ['./navbar-media-popover.component.scss'],
})
export class NavbarMediaPopoverComponent implements OnInit {

  @Input() onClick =() => {}
  constructor(private router: Router) {
  }

  ngOnInit() {
  }

  navigateAll() {
    this.router.navigate(['/navbar/all']).then(() => {
      this.onClick();
    });
  }

  navigateImages() {
    this.router.navigate(['/navbar/images']).then(() => {
      this.onClick();
    });
  }

  navigateVideos() {
    this.router.navigate(['/navbar/videos']).then(()=>{
      this.onClick();
    });
  }

}
