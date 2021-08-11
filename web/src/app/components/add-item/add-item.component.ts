import {Component, Input, OnInit} from '@angular/core';
import {PopoverController} from '@ionic/angular';

@Component({
  selector: 'app-add-tool',
  templateUrl: './add-item.component.html',
  styleUrls: ['./add-item.component.scss'],
})
export class AddItemComponent implements OnInit {

  @Input() availableItems: string[];
  @Input() title : string;
  private items: string[] = [];
  constructor(private popoverControl: PopoverController) { }

  ngOnInit() {
    // Nothing to add here yet

  }

  changeCheckbox(checked: boolean, tool: string) {
    if(checked){
      this.items.push(tool);
    } else {
      this.items= this.items.filter(t => t!== tool);
    }
  }

  addItems() {
    this.popoverControl.dismiss({items: this.items});
  }
}
