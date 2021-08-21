import {Component, OnInit} from '@angular/core';
import {MsalService} from '@azure/msal-angular';

@Component({
  selector: 'app-welcome-card',
  templateUrl: './welcome-card.component.html',
  styleUrls: ['./welcome-card.component.scss'],
})
export class WelcomeCardComponent implements OnInit {

  constructor(private msalService: MsalService) {
    //Nothing added here yet
  }

  ngOnInit() {
    //Nothing added here yet

  }


  /**
   * This function calls the loginRedirect method of the official microsoft authentication library, starting the userflow
   * for login/register/ forgot password
   */
  login() {
    this.msalService.loginRedirect();
  }

}
