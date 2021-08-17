import {Component, Input, OnInit} from '@angular/core';
import {PopoverController} from "@ionic/angular";

@Component({
  selector: 'app-media-filter',
  templateUrl: './media-filter.component.html',
  styleUrls: ['./media-filter.component.scss'],
})
export class MediaFilterComponent implements OnInit {
  segment : string = 'all';
  constructor(private popoverController : PopoverController) { }

  ngOnInit() {}

  async click(segment: string) {
    this.segment=segment;
    await this.popoverController.dismiss({segment: this.segment});
  }
}
