import {Component, OnInit} from '@angular/core';
import {MsalService} from '@azure/msal-angular';
import {Router} from '@angular/router';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss'],
})
export class AccountComponent implements OnInit {

  constructor(private msalService: MsalService, private router: Router) {
  }

  ngOnInit() {
  }

  logout() {
    this.msalService.logoutPopup();
    this.router.navigate(['/welcome']).then(() => {
      localStorage.clear();
    });
  }
}
