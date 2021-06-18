import { Component, OnInit } from '@angular/core';
import {ScreenSizeServiceService} from '../../services/screen-size-service.service';
import {PipelineModel} from '../../models/pipeline.model';
import {PipelineService} from '../../services/pipeline/pipeline.service';
import {ModalController} from '@ionic/angular';
import {AddPipelineComponent} from '../../components/add-pipeline/add-pipeline.component';

@Component({
  selector: 'app-analytics',
  templateUrl: './analytics.page.html',
  styleUrls: ['./analytics.page.scss'],
})
export class AnalyticsPage implements OnInit {
  public pipelineData: PipelineModel[];
  public pipelineCount: number;
  public isDesktop: boolean;
  constructor(private screenSizeService: ScreenSizeServiceService, public modalController: ModalController) {
    this.screenSizeService.isDesktopView().subscribe(isDesktop=>{
      this.isDesktop = isDesktop;
    });
  }

  ngOnInit() {
  }

  async openCreatePipelineModal(){
    try{
      const modal = await this.modalController.create({
        component : AddPipelineComponent,
        componentProps: {
          modal: this.modalController
        }
      });
      return await modal.present;
    }catch (e) {
     console.error(e);
    }
  }

  // const videoModal = await this.modal.create({
  //   component: VideostreamCardComponent,
  //   componentProps: {
  //     modal: this.modal
  //   }
  // })
  // videoModal.style.backgroundColor = "rgba(0,0,0,0.85)" //make the background for the modal darker.
  //
  // await videoModal.present();
}
