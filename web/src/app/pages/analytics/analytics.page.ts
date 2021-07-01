import { Component, OnInit } from '@angular/core';
import {ScreenSizeServiceService} from '../../services/screen-size-service.service';
import {Pipeline} from '../../models/pipeline';
import {PipelinesService} from '../../apis/pipelines.service';
import {GetPipelinesResponse} from '../../models/getPipelinesResponse';
import {ToastController} from '@ionic/angular';

@Component({
  selector: 'app-analytics',
  templateUrl: './analytics.page.html',
  styleUrls: ['./analytics.page.scss'],
})
export class AnalyticsPage implements OnInit {

  public isDesktop: boolean;
  public pipelines: Pipeline[] = [];

  constructor(private screenSizeService: ScreenSizeServiceService, private pipelinesService: PipelinesService,
              private toastController: ToastController) {
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

  editPipeline(pipeline: Pipeline){
  }

  ngOnInit() {
    this.pipelinesService.getPipelines().subscribe(response => {
      this.pipelines = response.pipelines;
    });
  }
}



