import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',

})

/**
 * Contains default labels that are used in the program.
 */
export class RegisterConstants {
  public labels = {
    email_address: 'Enter Email Address',
    verify_email: 'Verify',
    enter_code: 'Enter Verification Code',
    first_name: 'First Name',
    last_name: 'Surname',
    password: 'Password',
    password_confirm: 'Confirm Password'
  };
}
