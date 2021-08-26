import {Component, Input, OnInit} from '@angular/core';
import {MsalService} from '@azure/msal-angular';
import {Router} from '@angular/router';

@Component({
  selector: 'app-account-popover',
  templateUrl: './account-popover.component.html',
  styleUrls: ['./account-popover.component.scss'],
})
export class AccountPopoverComponent implements OnInit {

  constructor(private msalService: MsalService, private router: Router) {
  }

  @Input() onClick = () => {
  };

  ngOnInit() {
  }

  public logout() {
    this.onClick();
    this.msalService.logoutPopup();
    this.router.navigate(['/welcome']).then(() => {
      localStorage.clear();
    });
  }

  public displayAccountPreferencesModal() {

  }

}
