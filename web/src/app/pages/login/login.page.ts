import {Component, OnInit} from '@angular/core';

@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
})
export class LoginPage implements OnInit {
  public email: string;
  public password: string;
  public isIframe = false;

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
