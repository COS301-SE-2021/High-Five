import {Component, OnInit} from '@angular/core';
import {MsalService} from '@azure/msal-angular';
import {AuthenticationResult} from '@azure/msal-browser';

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

  login() {
    this.msalService.loginRedirect();
  }

}
