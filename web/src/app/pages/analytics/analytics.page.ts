import {Component, OnInit} from '@angular/core';
import {ScreenSizeServiceService} from '../../services/screen-size-service.service';
import {ModalController, ToastController} from '@ionic/angular';
import {AddPipelineComponent} from '../../components/add-pipeline/add-pipeline.component';
import {PipelineService} from '../../services/pipeline/pipeline.service';

@Component({
  selector: 'app-analytics',
  templateUrl: './analytics.page.html',
  styleUrls: ['./analytics.page.scss'],
})
export class AnalyticsPage implements OnInit {

  public isDesktop: boolean;

  constructor(private screenSizeService: ScreenSizeServiceService,
              private toastController: ToastController, private modalController: ModalController, public pipelineService: PipelineService) {
    this.screenSizeService.isDesktopView().subscribe(isDesktop => {
      this.isDesktop = isDesktop;
    });
  }

  public pipelinesTrackFn = (i, pipeline) => pipeline.id;

  ngOnInit() {
  }


  /**
   * Function opens a modal containing the add pipeline component, which will allow the user to create a pipeline
   */
  public async openAddPipelineModal() {
    const modal = await this.modalController.create({
      component: AddPipelineComponent,
      cssClass: 'add-pipeline-modal',
      showBackdrop: true,
      animated: true,
      backdropDismiss: false,
    });
    return modal.present();
  }

}



