import {Component, Input, OnInit} from '@angular/core';
import {PopoverController} from '@ionic/angular';

@Component({
  selector: 'app-add-tool',
  templateUrl: './add-item.component.html',
  styleUrls: ['./add-item.component.scss'],
})
export class AddItemComponent implements OnInit {

  @Input() availableItems: string[];
  @Input() title: string;
  /**
   * This array will be returned as it represents the selection the user made
   *
   * @private
   */
  private items: string[] = [];

  constructor(private popoverControl: PopoverController) {
  }

  ngOnInit() {
    // Nothing to add here yet

  }

  /**
   * This function adds checked items to the items array
   *
   * @param checked, a boolean to represent its checked value
   * @param item, the string value of the item
   */
  changeCheckbox(checked: boolean, item: string) {
    if (checked) {
      this.items.push(item);
    } else {
      this.items = this.items.filter(t => t !== item);
    }
  }

  addItems() {
    this.popoverControl.dismiss({items: this.items});
  }
}
