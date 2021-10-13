import {Component, OnInit} from '@angular/core';
import {OAuthService} from 'angular-oauth2-oidc';

@Component({
  selector: 'app-welcome-card',
  templateUrl: './welcome-card.component.html',
  styleUrls: ['./welcome-card.component.scss'],
})
export class WelcomeCardComponent implements OnInit {

  constructor(private oauthService: OAuthService) {
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
    this.oauthService.initImplicitFlow();
    // this.msalService.loginRedirect();
  }

}
