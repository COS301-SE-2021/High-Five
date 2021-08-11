import {Component, Input, OnInit} from '@angular/core';
import {PopoverController} from '@ionic/angular';

@Component({
  selector: 'app-add-tool',
  templateUrl: './add-tool.component.html',
  styleUrls: ['./add-tool.component.scss'],
})
export class AddToolComponent implements OnInit {

  @Input() availableTools: string[];
  private tools: string[] = [];
  constructor(private popoverControl: PopoverController) { }

  ngOnInit() {
    // Nothing to add here yet

  }

  changeCheckbox(checked: boolean, tool: string) {
    if(checked){
      this.tools.push(tool);
    } else {
      this.tools= this.tools.filter(t => t!== tool);
    }
  }

  addTools() {
    this.popoverControl.dismiss({tools: this.tools});
  }
}
