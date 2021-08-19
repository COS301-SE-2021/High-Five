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

  /**
   * Redirects the user to the all page
   */
  public navigateAll() {
    this.router.navigate(['/navbar/all']).then(() => {
      this.onClick();
    });
  }

  /**
   * Redirects the user to the images page
   */
  public navigateImages() {
    this.router.navigate(['/navbar/images']).then(() => {
      this.onClick();
    });
  }

  /**
   * Redirects the user to the videos page
   */
  public navigateVideos() {
    this.router.navigate(['/navbar/videos']).then(() => {
      this.onClick();
    });
  }

}
