import {Component, OnInit} from '@angular/core';
import {PopoverController} from '@ionic/angular';

@Component({
  selector: 'app-media-filter',
  templateUrl: './media-filter.component.html',
  styleUrls: ['./media-filter.component.scss'],
})
export class MediaFilterComponent implements OnInit {
  public segment = 'all';

  constructor(private popoverController: PopoverController) {
  }

  ngOnInit() {
  }

  public async onClick(segment: string) {
    this.segment = segment;
    await this.popoverController.dismiss({segment: this.segment});
  }
}
