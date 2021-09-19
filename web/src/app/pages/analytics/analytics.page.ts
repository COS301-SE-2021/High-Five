import {Component, OnInit} from '@angular/core';
import {ModalController, ToastController} from '@ionic/angular';
import {AddPipelineComponent} from '../../components/add-pipeline/add-pipeline.component';
import {PipelineService} from '../../services/pipeline/pipeline.service';

@Component({
  selector: 'app-analytics',
  templateUrl: './analytics.page.html',
  styleUrls: ['./analytics.page.scss'],
})
export class AnalyticsPage implements OnInit {


  constructor(private toastController: ToastController, private modalController: ModalController, public pipelineService: PipelineService) {
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



