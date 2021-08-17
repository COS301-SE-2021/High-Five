import {Component, OnInit, ViewChild} from '@angular/core';
import {ScreenSizeServiceService} from '../../services/screen-size-service.service';
import {Router} from '@angular/router';
import {MsalService} from '@azure/msal-angular';
import {VideosService} from "../../services/videos/videos.service";
import {ImagesService} from "../../services/images/images.service";
import {PipelinesService} from "../../apis/pipelines.service";
import {AnalyzedVideosService} from "../../services/analyzed-videos/analyzed-videos.service";
import {AnalyzedImagesService} from "../../services/analyzed-images/analyzed-images.service";
import {PopoverController} from "@ionic/angular";
import {NavbarMediaPopoverComponent} from "../../components/navbar-media-popover/navbar-media-popover.component";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.page.html',
  styleUrls: ['./navbar.page.scss'],
})
export class NavbarPage implements OnInit {

  @ViewChild('homeNav') homeNav: HTMLIonButtonElement;
  @ViewChild('analyticsNav') analyticsNav: HTMLIonButtonElement;
  @ViewChild('videoNav') videoNav: HTMLIonButtonElement;
  @ViewChild('controlsNav') controlsNav: HTMLIonButtonElement;
  //These links are arrays so that when the content is changed, it is shown in the HTML
  homeLink = ['active-link'];
  analyticsLink = ['link'];
  mediaLink = ['link'];
  controlsLink = ['link'];
  liveLink = ['link'];

  isDesktop: boolean;
  private navPages;


  constructor(private screenSizeService: ScreenSizeServiceService, private router: Router, private msalService: MsalService,
              private videosService: VideosService, private imagesService: ImagesService,
              private analyzedVideoService: AnalyzedVideosService, private analyzedImageService: AnalyzedImagesService,
              private pipelineService: PipelinesService, private popoverController: PopoverController) {
    this.screenSizeService.isDesktopView().subscribe(isDesktop => {
      this.isDesktop = isDesktop;
    });
    this.navPages = {
      homeNav: this.homeLink,
      analyticsNav: this.analyticsLink,
      videoNav: this.mediaLink,
      controlsNav: this.controlsLink
    };
  }

  ngOnInit() {
    //Nothing added here yet

  }

  logout() {
    this.msalService.logoutPopup();
    this.router.navigate(['/welcome']).then(() => {
      localStorage.removeItem('jwt');
    });
  }

  /**
   * This function will navigate the application to the provided url, as well as change the styling of the
   * navigation buttons to communicate to the user that they are at that url.
   *
   * @param url The url to navigate to
   * @param tab The navigation button to set to be active
   */
  navigateTo(url: string, tab: string) {
    // eslint-disable-next-line guard-for-in
    for (const key in this.navPages) {
      const value = this.navPages[key];
      if (key !== tab) {
        value[0] = 'link';
      } else {
        value[0] = 'active-link';
      }
    }
    this.router.navigate([url]);
  }

  async displayMediaPopover(ev: any) {
    const popoverComponent = await this.popoverController.create({
      component: NavbarMediaPopoverComponent,
      animated: true,
      translucent: true,
      backdropDismiss: true,
      event: ev,
      cssClass: 'navBarMediaPopover',
      showBackdrop: false,
      componentProps: {
        onClick: () => {
          popoverComponent.dismiss();
        }
      }
    });
    await popoverComponent.present();

  }

}
