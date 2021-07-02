import {Component, Input, OnInit} from '@angular/core';

@Component({
  selector: 'app-add-tool',
  templateUrl: './add-tool.component.html',
  styleUrls: ['./add-tool.component.scss'],
})
export class AddToolComponent implements OnInit {

  @Input() tools: string[];
  @Input() availableTools: string[];
  constructor() { }

  ngOnInit() {

  }

}
