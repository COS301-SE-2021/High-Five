import {AfterContentInit, Component, OnInit} from '@angular/core';
import { Platform } from '@ionic/angular';
import {RegisterConstants } from '../../../constants/register/constants-default'

@Component({
  selector: 'app-register',
  templateUrl: './register.page.html',
  styleUrls: ['./register.page.scss'],
})
export class RegisterPage implements OnInit, AfterContentInit {

  public showVerify = false;

  constructor(public constants: RegisterConstants, private platform: Platform) { }

  ngOnInit() {
  }

  ngAfterContentInit() {

  }

  /**
   * Determines the size of the device's screen and adjusts the given size accordingly.
   * If the screen size is less than 600px, multiply the given value by two, else return the value as is.
   * @param givenSize
   * @returns The adjusted value as a String
   */
  public adjustSize(givenSize :number) : string {
    let tmp: number = this.isMobile() ? givenSize * 2: givenSize;
    return String(tmp);
  }

  private isMobile() : boolean {
    return this.platform.width() < 600;
  }
}
