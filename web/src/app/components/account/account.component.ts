import {Component, OnInit} from '@angular/core';
import {ModalController} from '@ionic/angular';
import {MsalService} from '@azure/msal-angular';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss'],
})
export class AccountComponent implements OnInit {
  public option: string;


  constructor(private modalController: ModalController, public msalService: MsalService) {
    this.option = 'details';
  }

  ngOnInit() {
  }


  public async close() {
    await this.modalController.dismiss();
  }
}
