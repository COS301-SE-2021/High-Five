import {Component, Input, OnInit} from '@angular/core';
import {Router} from '@angular/router';

@Component({
  selector: 'app-navbar-media-popover',
  templateUrl: './navbar-media-popover.component.html',
  styleUrls: ['./navbar-media-popover.component.scss'],
})
export class NavbarMediaPopoverComponent implements OnInit {


  constructor(private router: Router) {
  }

  @Input() onClick = () => {
  };


  ngOnInit() {
  }

  public navigateAll() {
    this.router.navigate(['/navbar/all']).then(() => {
      this.onClick();
    });
  }

  public navigateImages() {
    this.router.navigate(['/navbar/images']).then(() => {
      this.onClick();
    });
  }

  public navigateVideos() {
    this.router.navigate(['/navbar/videos']).then(() => {
      this.onClick();
    });
  }

}
