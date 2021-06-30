import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',

})

/**
 * Contains default labels that are used in the program.
 */
export class RegisterConstants {
  public labels = {
    emailAddress: 'Enter Email Address',
    verifyEmail: 'Verify',
    enterCode: 'Enter Verification Code',
    firstName: 'First Name',
    lastName: 'Surname',
    password: 'Password',
    passwordConfirm: 'Confirm Password'
  };
}
