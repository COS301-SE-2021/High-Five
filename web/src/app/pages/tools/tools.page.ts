import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {UsersService} from '../../services/users/users.service';

@Component({
  selector: 'app-tools',
  templateUrl: './tools.page.html',
  styleUrls: ['./tools.page.scss'],
})
export class ToolsPage implements OnInit {
  public page: string;

  constructor(private router: Router, public usersService: UsersService) {
    this.page = 'approved';
  }

  ngOnInit() {
  }

  segmentChange(ev: any) {
    this.router.navigate(['/navbar/tools/' + this.page]);
  }
}
