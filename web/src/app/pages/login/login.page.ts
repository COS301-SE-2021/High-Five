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

  constructor(private authService: MsalService) {
    //Nothing added here yet
    this.isIframe = window !== window.parent && !window.opener;

  }

  login() {
    this.authService.loginPopup().subscribe(
      {
        next: (result) => {
          console.log(result);
        },
        error: (error) => console.log(error)
      }
    );
  }

  ngOnInit() {
    //Nothing added here yet

  }
}
