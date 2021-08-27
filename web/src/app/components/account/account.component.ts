import {Component, OnInit} from '@angular/core';
import {ModalController} from '@ionic/angular';
import {MsalService} from '@azure/msal-angular';
import {UsersService} from '../../services/users/users.service';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss'],
})
export class AccountComponent implements OnInit {
  public option: string;


  constructor(private modalController: ModalController, public msalService: MsalService, public usersService: UsersService) {
    this.option = 'details';
  }

  ngOnInit() {
  }


  public async close() {
    await this.modalController.dismiss();
  }
}
