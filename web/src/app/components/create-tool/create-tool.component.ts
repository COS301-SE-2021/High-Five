import {Component, OnInit} from '@angular/core';

@Component({
  selector: 'app-create-tool',
  templateUrl: './create-tool.component.html',
  styleUrls: ['./create-tool.component.scss'],
})
export class CreateToolComponent implements OnInit {

  public toolName: string;
  public toolType: string;
  public metaData: string;

  constructor() {
  }

  ngOnInit() {
  }
}
