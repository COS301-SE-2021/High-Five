import { Component, OnInit } from '@angular/core';
import {ScreenSizeServiceService} from '../../services/screen-size-service.service';
import {Pipeline} from '../../models/pipeline';
import {PipelinesService} from '../../apis/pipelines.service';
import {ModalController, ToastController} from '@ionic/angular';
import {EditPipelineComponent} from '../../components/edit-pipeline/edit-pipeline.component';
import {AddPipelineComponent} from '../../components/add-pipeline/add-pipeline.component';

@Component({
  selector: 'app-analytics',
  templateUrl: './analytics.page.html',
  styleUrls: ['./analytics.page.scss'],
})
export class AnalyticsPage implements OnInit {

  public isDesktop: boolean;
  public pipelines: Pipeline[] = [];

  constructor(private screenSizeService: ScreenSizeServiceService, private pipelinesService: PipelinesService,
              private toastController: ToastController, private modalController: ModalController) {
    this.screenSizeService.isDesktopView().subscribe(isDesktop => {
      this.isDesktop = isDesktop;
    });
  }

  deletePipeline(id: string){
    this.pipelines = this.pipelines.filter(pipeline => pipeline.id !==id);
    const  toast = this.toastController.create({
      message: 'Successfully deleted pipeline',
      duration: 2000,
      translucent : true
    }).then(m => m.present());
  }

  removeTool(pipeline: Pipeline){
    const  toast = this.toastController.create({
      header: 'Success!',
      message: 'removed tool from ' + pipeline.name,
      duration: 2000,
      translucent : true,
      position: 'bottom'
    }).then(m => m.present());
  }

  ngOnInit() {
    this.pipelinesService.getPipelines().subscribe(response => {
      this.pipelines = response.pipelines;
    });
  }


  async openAddPipelineModal(){
    const modal = await this.modalController.create({
      component: AddPipelineComponent,
      cssClass: 'add-pipeline-modal',
      showBackdrop: true,
      animated: true
    });
    modal.onWillDismiss().then(data=> {
      console.log(data.data.pipeline);
    });
    return await modal.present();
  }
  addPipeline() {

  }
}



