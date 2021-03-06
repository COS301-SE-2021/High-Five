import {Component, OnDestroy, OnInit} from '@angular/core';
import {AnimationOptions} from 'ngx-lottie';
import {LiveStreamingService} from '../../services/live-streaming/live-streaming.service';
import {UserPreferencesService} from '../../services/user-preferences/user-preferences.service';
import {PopoverController} from '@ionic/angular';
import {LiveAnalysisFilterComponent} from '../../components/live-analysis-filter/live-analysis-filter.component';
import {PipelineService} from '../../services/pipeline/pipeline.service';
import {DomSanitizer} from '@angular/platform-browser';
import {environment} from '../../../environments/environment';


@Component({
  selector: 'app-live',
  templateUrl: './live.page.html',
  styleUrls: ['./live.page.scss'],
})
export class LivePage implements OnInit, OnDestroy {
  public baseUrl: string;

  /**
   * The configuration of the lottie animation on this page (not present currently)
   */
  public lottieConfig: AnimationOptions = {
    path: '/assets/lottie-animations/67783-drones-isometric-lottie-animation.json'
  };

  constructor(public liveStreamingService: LiveStreamingService, public userPreferencesService: UserPreferencesService,
              private popoverController: PopoverController, public pipelineService: PipelineService,
              public domSanitizer: DomSanitizer) {
    this.baseUrl = environment.streamPlayBaseUrl;
  }

  ngOnInit() {
  }


  public async displaySelectPipelinePopover(ev: any) {
    const filterPopover = await this.popoverController.create({
      component: LiveAnalysisFilterComponent,
      cssClass: 'media-filter',
      animated: true,
      translucent: true,
      backdropDismiss: true,
      event: ev,
    });
    await filterPopover.present();
    await filterPopover.onDidDismiss().then(
      data => {
        /**
         * The below is to ensure the popover isn't dismissed from the backdrop, in which case the data.data and
         * data.data.segment will be undefined.
         */
        if (data.data !== undefined) {
          if (data.data.pipeline !== undefined) {
            this.userPreferencesService.updateLiveAnalysisPipeline(data.data.pipeline);
          }
        }
      }
    );
  }

  ngOnDestroy(): void {
  }

  public getLink(id: string) {
    return this.domSanitizer.bypassSecurityTrustResourceUrl(environment.streamPlayBaseUrl +
      this.liveStreamingService.appName + '/play.html?name=' + id);
  }

  refreshLive() {
    this.liveStreamingService.fetchAll();
  }
}
