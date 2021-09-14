import {Component, OnInit} from '@angular/core';
import {UserToolsService} from '../../services/user-tools/user-tools.service';
import {ModalController} from '@ionic/angular';

@Component({
  selector: 'app-create-tool',
  templateUrl: './create-tool.component.html',
  styleUrls: ['./create-tool.component.scss'],
})
export class CreateToolComponent implements OnInit {

  public toolName: string;
  public toolType: string;
  public metaData: string;
  public toolClassFile: any;
  public toolModelFile: any;

  constructor(public userToolsService: UserToolsService, private modalController: ModalController) {
    this.metaData = 'BoxCoordinates';
    this.toolType = 'drawing';
  }

  public toolTypeTrackFn = (t, tool) => tool.id;


  public createTool() {
    if (this.toolType === 'analysis') {
      this.userToolsService.addAnalysisTool(this.toolClassFile, this.toolModelFile, this.metaData, this.toolName);
    } else if (this.toolType === 'drawing') {
      this.userToolsService.addDrawingTool(this.toolClassFile, this.metaData, this.toolName);
    }
    this.modalController.dismiss({dismissed: true});
  }

  public async cancel() {
    await this.modalController.dismiss({dismissed: true});
  }

  public uploadClassFile(ev: any) {
    this.toolClassFile = ev.target.files[0];
  }

  public uploadModel(ev: any) {
    this.toolModelFile = ev.target.files[0];
  }

  ngOnInit() {
  }
}
