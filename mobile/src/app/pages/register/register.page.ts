import { Component, OnInit } from '@angular/core';
import { Platform } from '@ionic/angular';
import {RegisterConstants } from '../../../constants/register/constants-default'

@Component({
  selector: 'app-register',
  templateUrl: './register.page.html',
  styleUrls: ['./register.page.scss'],
})
export class RegisterPage implements OnInit {

  public showVerify = false;

  constructor(public constants: RegisterConstants, private platform: Platform) { }

  ngOnInit() {
  }

  /**
   * Determines the size of the
   * @param givenSize
   */
  public adjustSize(givenSize :number) : string {
    let tmp: number = this.platform.width() >= 600 ? givenSize : givenSize * 2;
    return String(tmp);
  }
}
