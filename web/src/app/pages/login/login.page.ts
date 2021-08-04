import {Component, OnInit} from '@angular/core';
import {MsalService} from '@azure/msal-angular';

@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
})
export class LoginPage implements OnInit {
  email: string;
  password: string;
  isIframe = false;

  constructor() {
    //Nothing added here yet
    this.isIframe = window !== window.parent && !window.opener;

  }

  login() {
  }

  ngOnInit() {
    //Nothing added here yet

  }
}
