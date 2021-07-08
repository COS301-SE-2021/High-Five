import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Pipeline} from '../../models/pipeline';
import {convertNodeSourceSpanToLoc} from '@angular-eslint/template-parser/dist/convert-source-span-to-loc';
import {LoadingController, Platform, PopoverController, ToastController} from '@ionic/angular';
import {element} from 'protractor';
import {PipelinesService} from '../../apis/pipelines.service';
import {DeletePipelineRequest} from '../../models/deletePipelineRequest';
import {RemoveToolsRequest} from '../../models/removeToolsRequest';
import {AddToolComponent} from '../add-tool/add-tool.component';

@Component({
  selector: 'app-pipeline',
  templateUrl: './pipeline.component.html',
  styleUrls: ['./pipeline.component.scss'],
})
export class PipelineComponent implements OnInit {
  @Input() pipeline: Pipeline;
  @Input() availableTools: string[];
  @Output() deletePipeline: EventEmitter<string> = new EventEmitter<string>(); // Will send the id of the pipeline
  @Output() removeTool: EventEmitter<Pipeline> = new EventEmitter<Pipeline>(); // Will send through a new pipeline object
  constructor(private platform: Platform, private pipelinesService: PipelinesService,
              private loadingController: LoadingController, private toastController: ToastController,
              private popoverController: PopoverController) {
  }

  ngOnInit() {
    /**
     * The below allows us to add custom colours to the ionic chips, you cant use color ={{color}} as by default the
     * chips only accept the ionic colours (primary, secondary, warning, etc.)
     */
    this.platform.ready().then(() => {
      this.updateToolColours();
    });
  }

  async onRemoveTool(tool: string) {
    if(this.pipeline.tools.length===1){
      await this.onDeletePipeline();
    }
    const removeToolRequest: RemoveToolsRequest = {
      pipelineId: this.pipeline.id,
      tools: [tool]
    };
    try {
      const rest = this.pipelinesService.removeTools(removeToolRequest).subscribe(response => {
        this.pipeline.tools = this.pipeline.tools.filter(t => t !== tool);
        this.removeTool.emit(this.pipeline);
      });
    } catch (e) {
      const toast = await this.toastController.create({
        message: 'Removal of tool from pipeline failed',
        duration: 2000
      });
      await toast.present();
    }
  }

  async onAddTool(tools: string[]) {
    this.pipeline.tools= this.pipeline.tools.concat(tools);
    this.pipelinesService.addTools({pipelineId: this.pipeline.id,tools : this.pipeline.tools}).subscribe(this.updateToolColours);
  }

  async onDeletePipeline() {
    const loading = await this.loadingController.create({
      spinner: 'dots',
      animated: true,
    });
    await loading.present();

    const deletePipelineRequest: DeletePipelineRequest = {
      pipelineId: this.pipeline.id
    };
    try {
      const res = this.pipelinesService.deletePipeline(deletePipelineRequest).subscribe(response => {
        loading.dismiss();
        this.deletePipeline.emit(this.pipeline.id);
      });
    } catch (e) {
      const toast = await this.toastController.create({
        message: 'Deletion of pipeline failed, please contact the developers',
        duration: 2000
      });
      await loading.dismiss();
      await toast.present();
    }
    await loading.dismiss();

    this.deletePipeline.emit(this.pipeline.id);
  }

  async presentAddToolPopover(ev: any) {
    const addToolPopover = await this.popoverController.create({
      component: AddToolComponent,
      event: ev,
      translucent: true,
      componentProps: {
        availableTools: this.availableTools.filter(tool => !this.pipeline.tools.includes(tool))
      }
    });
    await addToolPopover.present();
    await addToolPopover.onDidDismiss().then(
      data=>{
        this.onAddTool(data.data.tools);
      }
  );
  }

  private updateToolColours(){
    const temp = Array.from(document.getElementsByClassName('tool-chip') as HTMLCollectionOf<HTMLElement>);
    temp.forEach(value => {
      value.style.borderColor = '#' + ('000000' +
        Math.floor(0x1000000 * Math.random()).toString(16)).slice(-6);
    });
  }
}
