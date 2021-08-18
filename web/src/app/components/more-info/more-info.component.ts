import {Component, Input, OnInit} from '@angular/core';

@Component({
  selector: 'app-more-info',
  templateUrl: './more-info.component.html',
  styleUrls: ['./more-info.component.scss'],
})
export class MoreInfoComponent implements OnInit {
  @Input() title: string;
  @Input() description: string;

  constructor() {
  }

  ngOnInit() {
  }

}
