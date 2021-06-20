import {Component, OnInit} from '@angular/core';
import {ToolsetConstants} from '../../../constants/toolset-constants';
import {PipelinesService} from '../../apis/pipelines.service';
import {Pipeline} from '../../models/pipeline';
import {CreatePipelineRequest} from '../../models/createPipelineRequest';
import {LoadingController, ToastController} from '@ionic/angular';
import {PipelineComponent} from '../pipeline/pipeline.component';
import {PipelineService} from '../../services/pipeline/pipeline.service';

@Component({
  selector: 'app-add-pipeline',
  templateUrl: './add-pipeline.component.html',
  styleUrls: ['./add-pipeline.component.scss'],
  providers: [PipelineComponent]
})

/**
 * This class serves as a way to add pipelines
 */
export class AddPipelineComponent implements OnInit {
  selectedTools: boolean[];
  pipelineName: string;
  constructor(public constants: ToolsetConstants, public pipelinesService: PipelinesService,
              private loadingController: LoadingController, private toastController: ToastController,
              private pipelineService: PipelineService) {
    this.selectedTools = new Array<boolean>(this.constants.labels.tools.length);
  }


  /**
   * The function will make a request to the backend to create a new pipeline with the selected tools and name,
   * this is accomplished using OpenAPI requests and responses and using these as parameters and return types for the
   * pipelinesService
   */
  async addPipeline(){
    const loading = await this.loadingController.create({
      spinner: 'circles',
      animated:true,
    });
    await loading.present();
    const toast = await  this.toastController.create( // A small informative message at the bottom of the screen
      {
        message: 'Pipeline successfully created',
        duration: 2000
      }
    );
    toast.translucent=true; // Will only work on IOS
    const temp: string[] = [];
    let allEmpty = true;
    for (let i = 0; i < this.selectedTools.length; i++) {
      if(this.selectedTools[i]){
        temp.push(this.constants.labels.tools[i]);
        allEmpty= false;
      }
    }
    if(allEmpty || this.pipelineName==' '){
      await loading.dismiss();
      toast.message = 'Pipeline must contain a name and have at least one tool selected before creation';
      await toast.present();
      return;
    }

    const newPipeline: Pipeline ={
      name:this.pipelineName,
      tools: temp,
    };

    const newPipelineRequest: CreatePipelineRequest = {
      pipeline: newPipeline,
    };

    const res = this.pipelinesService.createPipeline(newPipelineRequest).subscribe(
      response =>{
        if (!response.success){
          toast.message = 'Error occurred whilst creating pipeline';
        }
        this.pipelineName='';
        const allCheckBoxes = document.querySelectorAll('ion-checkbox');
        // eslint-disable-next-line @typescript-eslint/prefer-for-of
        for(let i =0; i < allCheckBoxes.length; i++){
          allCheckBoxes[i].checked=false;
        }
        loading.dismiss();
        toast.present();
        this.pipelineService.setNewPipelineAdded(true);
      }
    );

  }
  ngOnInit() {}
}
