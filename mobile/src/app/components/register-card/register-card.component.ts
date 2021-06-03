import { Component, OnInit } from '@angular/core';
import {RegisterConstants} from "../../../constants/register-constants";
import {Platform} from "@ionic/angular";

@Component({
  selector: 'app-register-card',
  templateUrl: './register-card.component.html',
  styleUrls: ['./register-card.component.scss'],
})
export class RegisterCardComponent implements OnInit {

  public showVerify = false;  //if true, the 'verification code' input is shown

  constructor(public constants: RegisterConstants, private platform: Platform) { }

  ngOnInit() {}

  /**
   * Determines the size of the device's screen and adjusts the given size accordingly.
   * If the screen size is less than 600px, multiply the given value by two, else return the value as is.
   * @param givenSize The relative size of the column
   * @returns The adjusted value as a String
   */
  public adjustSize(givenSize :number) : string {
    let tmp : number = this.isMobile() ? givenSize * 2 : givenSize;

    //limit tmp to 12
    if (tmp > 12) {
      tmp = 12;
    }

    if (tmp < 1) {
      tmp = 1;
    }

    return String(tmp);
  }

  /**
   * Determine if the device is possibly a mobile device based on the screen size.
   * @private
   */
  private isMobile() : boolean {
    return this.platform.width() < 700;
  }

}
