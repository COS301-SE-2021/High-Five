import {Component, OnInit} from '@angular/core';
import {MetaData} from '../../models/metaData';

@Component({
  selector: 'app-create-tool',
  templateUrl: './create-tool.component.html',
  styleUrls: ['./create-tool.component.scss'],
})
export class CreateToolComponent implements OnInit {

  public toolName: string;
  public toolType: string;
  public metaData: string;
  public newMetaData: MetaData = {name: undefined, id: undefined, classFile: undefined};

  constructor() {
  }

  ngOnInit() {
  }

  uploadMetadataFile(event: any) {
    this.newMetaData.classFile = event.target.files[0];
  }

  updatedNewMetaDataRow() {
    if (this.metaData === 'create one now') {
    }
  }
}
